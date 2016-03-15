#include "ByteBuffer.h"

ByteBuffer buffer;
volatile long timeCounter = 0;
long timeCounterMax = 512;
void (*whichLoop)();
void (*whichTimer)();
void (*whichSetup)();

#define PIN10_LOW() (PORTB &= ~_BV(PORTB2))
#define PIN10_HIGH() (PORTB |= _BV(PORTB2))
#define PIN10_TOGGLE() (PORTB ^= _BV(PORTB2))
#define PIN10_VALUE() (PINB & _BV(PINB2))

#define PIN13_LOW() (PORTB &= ~_BV(PORTB5))
#define PIN13_HIGH() (PORTB |= _BV(PORTB5))
#define PIN13_TOGGLE() (PORTB ^= _BV(PORTB5))
#define PIN13_VALUE() (PINB & _BV(PINB5))

uint8_t GetBitAt(unsigned char b, unsigned char bit)
{
  return !!(b & (1 << bit));  
}

uint8_t str[13] = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, '\n', 0};
//'h', 'e', 'l', 'l', 'o', ' ', 'w', 'o', 'r', 'l', 'd', 0

//runt om de 4microsec[prescaler=64]
bool SendBit(bool vvv)
{
    static uint8_t current_pulse = 0;
    if(++current_pulse <= 7)
    {
        if(current_pulse == 1)
        {
            if(vvv)
            {
                PORTB |= _BV(PORTB2);
            }
            else
            {
                PORTB &= ~_BV(PORTB2);
            }        
        }
        return false;
    }

    current_pulse = 0;
    return true;
}

bool SendByte(uint8_t b)
{
    static uint8_t current_state = 0;
    static uint8_t current_bit = 0;
    static uint8_t parity = 0;
    
    switch(current_state)
    {
    case 0:
        if(SendBit(0))
        {
            current_state = 1;
        }
        break;
    case 1:
        if(current_bit < 8)
        {
            bool vvv = GetBitAt(b, current_bit);
            if(SendBit(vvv))
            {
                if(vvv)
                {
                    parity ^= 1;
                }
                if(++current_bit == 8)
                {
                    current_bit = 0;
                    current_state = 2;
                }
            }
        }
        break;
    case 2:
        if(SendBit(parity))
        {
            parity = 0;
            current_state = 3;
        }
        break;
    case 3:
        if(SendBit(1))
        {
            current_state = 0;
            return true;
        }
        break;
    }
    return false; 
}

void ProcessSend()
{
    PORTB ^= _BV(PORTB1);
    
    static int sending = -1;
    if(sending == -1)
    {
        if(buffer.getSize())
        {
            sending = buffer.get();
        }
    }
    else
    {
        if(SendByte(sending))
        {
            Serial.print("Sent: ");
            Serial.println(sending);
            sending = -1;
        }
    }
}

void ReceivedByte(uint8_t b, uint8_t errorOccured, uint8_t parityOK, uint8_t stopOK)
{
    Serial.print("Byte: ");
    Serial.print((int)b);
    if(errorOccured)
    {
        if(!parityOK)
        {
            Serial.print(" | parity error occured");
        }
    
        if(!stopOK)
        {
            Serial.print(" | stop error occured");
        }
    }
    timeCounterMax = b * 2;
    Serial.println("");
}

void ProcessReceive()
{
    static const uint8_t pulseMaxCount = 8;
    static uint8_t pulseArray[pulseMaxCount];    
    static uint8_t pulseCount = 0;
    
    static uint8_t readBit = 0;
    static uint8_t reading = 0;
    static uint8_t readByte = 0;
    static uint8_t readBitCount = 0;
    
    static uint8_t parityCheck = 0;
    
    pulseArray[pulseCount++] = PIN10_VALUE();
    if(pulseCount != pulseMaxCount)
    {
        return;
    }
    
    pulseCount = 0;
    readBit = (pulseArray[4] + pulseArray[5] + pulseArray[6]) / 2;

    if(!reading && !readBit)
    {
        readByte = 0;
        readBitCount = 0;
        parityCheck = 0;
        reading = 1;
    }
    else if(reading)
    {
        if(readBitCount >= 0 && readBitCount < 8)
        {
            if(readBit)
            {
                parityCheck ^= 1;
            }
            readByte ^= (-readBit ^ readByte) & (1 << readBitCount); //setBit(readByte, readBitCount, readBit);
        }
        else if(readBitCount == 8)
        {
            parityCheck = readBit == parityCheck;
        }
        else if(readBitCount == 9)
        {
            //ReceivedByte(readByte, !(parityCheck && readBit), parityCheck, readBit);
            buffer.put(readByte);
            reading = 0;
        }
        ++readBitCount;
    }
}

ISR(TIMER1_COMPA_vect)
{
    ++timeCounter;
    whichTimer();
}

void SetupSend()
{
    pinMode(10, OUTPUT); 
    digitalWrite(10,LOW);
}

void SetupReceive()
{
  pinMode(13, OUTPUT);
  pinMode(10,INPUT_PULLUP);
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
    
    // Prescaler 1024
    TCCR1B |= _BV(CS10) | _BV(CS12);
    
    // Timer interrupt enable
    TIMSK1 = 0;
    TIMSK1 |= _BV(OCIE1A);
    
    OCR1A = 256; // 1 = 64 microseconds
    // enable global interrupts
    sei(); 
}

void SetupSyncSend()
{
    PIN10_LOW();
    long start_time = millis();
    while(millis() - start_time < 5000)
    { }

    PIN10_HIGH();
    while(!(PINB & _BV(PINB2)))
    { }      
    while(!(PINB & _BV(PINB2)))//this is no error lol
    { }    

    PerformInterruptSetup();

    whichLoop = LoopSend;  
     
    Serial.println("S: Sync Executed");
}

void SetupSyncReceive()
{
    while(!(PINB & _BV(PINB2)))
    { }

    PerformInterruptSetup();

    whichLoop = LoopReceive;
    
    Serial.println("R: Sync Executed");
}

void LoopSend()
{
    if(timeCounter > 33) // ~528ms
    {
        timeCounter = 0;
        buffer.put(170);
        //buffer.put(analogRead(A0) / 5);//1024/8 = 128 max
        Serial.print("Sending: ");
        Serial.println(analogRead(A0) / 5);
    }
}

void LoopReceive()
{
    while(buffer.getSize())
    {
        ReceivedByte(buffer.get(), 0, 0, 0);
    }
    if(timeCounter > timeCounterMax)
    {
        timeCounter = 0;
        PIN13_TOGGLE();
    }
}

void setup() 
{  
    Serial.begin(9600);
    pinMode(A0, INPUT_PULLUP);
    
    timeCounterMax = analogRead(A0);
    buffer.init(1024);
    
    if(timeCounterMax > 64)
    {
        Serial.println("Entering SEND mode");
        whichLoop = SetupSyncSend;
        whichTimer = ProcessSend;
        whichSetup = SetupSend;
    }
    else
    {
        timeCounterMax = 100;
        Serial.println("Entering RECV mode");
        whichLoop = SetupSyncReceive;
        whichTimer = ProcessReceive;
        whichSetup = SetupReceive;
    }
    
    whichSetup();
}

void loop() 
{
    whichLoop();
}
