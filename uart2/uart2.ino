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

#define PIN13_LOW() (PORTB &= ~_BV(PORTB5))
#define PIN13_HIGH() (PORTB |= _BV(PORTB5))
#define PIN13_TOGGLE() (PORTB ^= _BV(PORTB5))
#define PIN13_VALUE() (PINB & _BV(PINB5))

#define SET_BIT(var, n, value) (var ^= (-value ^ var) & (1 << n))
#define ENABLE_CHANGE_INTERRUPT_0() (EICRA = (EICRA & ~((1 << ISC00) | (1 << ISC01))) | (1 << ISC00), EIMSK |= (1 << INT0)) //pin 2

uint8_t GetBitAt(unsigned char b, unsigned char bit)
{
  return !!(b & (1 << bit));  
}

uint8_t str_pos = 0;
uint8_t str[13] = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, '\r', '\n'};
//'h', 'e', 'l', 'l', 'o', ' ', 'w', 'o', 'r', 'l', 'd', '\r', '\n'

volatile long receive_timer = 0;

ISR(INT0_vect)
{
    static bool receiving = false;
    static uint8_t data = 0;
    static uint8_t pos = 0;  

    if(!receiving)
    {
        //lo to hi | rise | not receiving
        receive_timer = 0;
    }
    else //if(receiving)
    {
        //hi to lo | fall | receiving
        //long = '1', short = '0'
                    
        if(receive_timer > 6)
        {
            data |= 1 << pos;         
        }

        if(++pos == 8)
        {         
            buffer.put(data);
            data = 0;
            pos = 0;          
        }
    }

    receiving ^= 1;
}

void ProcessReceive()
{
    //done in interrupt
}

bool SendBit(bool bit)
{
    static bool sending = false;
    if(!sending)
    {
        receive_timer = 0;
        sending = true;
        PIN2_HIGH();
    }
    else if(sending && receive_timer > 2)
    {
        if(!bit || (bit && receive_timer > 9))
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

    if(SendBit(GetBitAt(b, current_bit)) && ++current_bit == 8)
    {
        current_bit = 0;
        return true;
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
    OCR1A = 256; // 1 = 62.5 nanoseconds
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
    buffer.init(1024);
    
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

