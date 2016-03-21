#include "ByteBuffer.h"
volatile int timeCounterMax = 0;

ByteBuffer buffer;

void (*whichLoop)();
void (*whichTimer)();
void (*whichSetup)();

#define PIN2_LOW() (PORTD &= ~_BV(PORTD2))
#define PIN2_HIGH() (PORTD |= _BV(PORTD2))
#define PIN2_TOGGLE() (PORTD ^= _BV(PORTD2))
#define PIN2_VALUE() (PIND & _BV(PIND2))

#define PIN10_LOW() (PORTB &= ~_BV(PORTB5))
#define PIN10_HIGH() (PORTB |= _BV(PORTB5))
#define PIN10_TOGGLE() (PORTB ^= _BV(PORTB5))
#define PIN10_VALUE() (PINB & _BV(PINB5))

#define PIN13_LOW() (PORTB &= ~_BV(PORTB5))
#define PIN13_HIGH() (PORTB |= _BV(PORTB5))
#define PIN13_TOGGLE() (PORTB ^= _BV(PORTB5))
#define PIN13_VALUE() (PINB & _BV(PINB5))

#define SET_BIT(var, n, value) (var ^= (-value ^ var) & (1 << n))
#define ENABLE_CHANGE_INTERRUPT_0() (EICRA = (EICRA & ~((1 << ISC00) | (1 << ISC01))) | (1 << ISC00), EIMSK |= (1 << INT0)) //pin 2

bool GetBitAt(unsigned char b, unsigned char bit)
{
    return !!(b & (1 << bit));
}

uint8_t str_pos = 0;
uint8_t str[13] = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, '\r', '\n'};
//'h', 'e', 'l', 'l', 'o', ' ', 'w', 'o', 'r', 'l', 'd', '\r', '\n'

volatile long receive_timer = 0;

const int LENGTH_START_BIT = 20;
const int LENGTH_HIGH_BIT = 10;
const int LENGTH_LOW_BIT = 0;
const int LENGHT_START_BIT_SEND = LENGTH_START_BIT + 4;
const int LENGHT_HIGH_BIT_SEND = LENGTH_HIGH_BIT + 4;
const int LENGTH_LOW_BIT_SEND = LENGTH_LOW_BIT + 3;

ISR(INT0_vect)
{
    static bool receiving = false;
    static bool current_state = LOW;
    static uint8_t data = 0;
    static uint8_t pos = 0;  
    static bool parity = 0;

    current_state ^= 1;
    
    if(current_state) //oldstate == low
    {
        receive_timer = 0;
    }
    else //oldstate == high
    {
        if(!receiving && receive_timer >= LENGTH_START_BIT)
        {
            receiving = true;
        }
        else if(receiving)
        {
            if(pos++ < 8)
            {
                if(receive_timer >= LENGTH_HIGH_BIT)
                {
                    data |= 1 << pos;       
                    parity ^= 1;  
                }
            }
            else
            {
                if(receive_timer >= LENGTH_HIGH_BIT && parity || !parity)
                {
                    buffer.put(data);
                }
                
                data = 0;
                pos = 0;
                parity = 0;
            }
        }
    }
}

void ProcessReceive()
{
    //done in interrupt
}

bool SendBit(bool bit, bool startbit = false)
{
    static bool sending = false;
    if(!sending)
    {
        receive_timer = 0;
        sending = true;
        PIN2_HIGH();
    }
    else if(sending)
    {
        if((!startbit && (!bit || (bit && receive_timer >= LENGHT_HIGH_BIT_SEND))) || (startbit && receive_timer >= LENGHT_START_BIT_SEND))
        {
            PIN2_LOW();
            sending = false;
            return true;
        }
    }

    return false;
}

bool SendByte(uint8_t b)
{
    static uint8_t current_bit = 0;
    static bool parity = 0;
    
    if(current_bit == 0 && SendBit(false, true)) // start bit
    {
        ++current_bit;
    }
    else if(current_bit > 0)
    {
        bool bit = GetBitAt(b, current_bit - 1);
        if(SendBit(bit) && ++current_bit < 9)
        {
            if(bit)
            {
                parity ^= 1;
            }
        }
        else if(current_bit == 9 && SendBit(parity))
        {
            current_bit = 0;
            parity = 0;
            return true;
        }
    }
   
    return false; 
}

void ProcessSend()
{
   static int sending = -1;
    if(sending == -1 && buffer.getSize())
    {
        sending = buffer.get();
    }
    else if(SendByte(sending))
    {
        sending = buffer.getSize() ? buffer.get() : -1;
    }   
}

ISR(TIMER1_COMPA_vect)
{
    PORTB ^= _BV(PORTB1);    
    ++receive_timer;
    whichTimer();
}

void SetupSend()
{
    pinMode(13, OUTPUT);
    pinMode(2, OUTPUT); 
    
    digitalWrite(2,LOW);
    digitalWrite(13, HIGH);
    delay(5000);
    digitalWrite(13, LOW);
}

void SetupReceive()
{
    pinMode(13, OUTPUT);
    pinMode(2,INPUT_PULLUP);
    ENABLE_CHANGE_INTERRUPT_0();
}

void PerformInterruptSetup()
{
    // disable interrupts
    cli();
    
    // clear registers
    TCCR1A = 0;
    TCCR1B = 0;
    
    // CTC Mode
    TCCR1B |= _BV(WGM12);
    
    // Prescaler 1
    TCCR1B |= _BV(CS10);
    
    // Timer interrupt enable
    TIMSK1 = 0;
    TIMSK1 |= _BV(OCIE1A);

   //kleiner getal dan dit en het werkt niet meer [256], with the current number config we have 5681 kib/s:
    OCR1A = 2048; // 1 = 62.5 nanoseconds
    // enable global interrupts
    sei(); 
}

//keep sending hello world\r\n
void LoopSend()
{
    if(buffer.put(str[str_pos]))
    {
        if(++str_pos == 13)
        {
            str_pos = 0;
        }
    }
}

void LoopReceive()
{
    while(buffer.getSize())
    {
        Serial.write(buffer.get());
    }
}

void setup() 
{  
    Serial.begin(250000);
    pinMode(A0, INPUT_PULLUP);
    
    timeCounterMax = analogRead(A0);
    buffer.init(32);
    
    if(timeCounterMax > 64)
    {
        Serial.println("Entering SEND mode");
        whichLoop = LoopSend;
        whichTimer = ProcessSend;
        whichSetup = SetupSend;
    }
    else
    {
        timeCounterMax = 100;
        Serial.println("Entering RECV mode");
        whichLoop = LoopReceive;
        whichTimer = ProcessReceive;
        whichSetup = SetupReceive;
    }

   
    whichSetup();

    PerformInterruptSetup();
}

void loop() 
{
    whichLoop();
}

