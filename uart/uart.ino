#define TX (13)
#define RX (12)

#include "ByteBuffer.h"
ByteBuffer sendBuffer;
ByteBuffer recvBuffer;

uint8_t GetBitAt(unsigned char b, unsigned char bit)
{
  return !!(b & (1 << bit));  
}


uint8_t str[13] = { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, '\n', 0};

//'h', 'e', 'l', 'l', 'o', ' ', 'w', 'o', 'r', 'l', 'd', 0
uint8_t current_idx = 0;
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
                PORTB |= _BV(PORTB5);
            }
            else
            {
                PORTB &= ~_BV(PORTB5);
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
    
    static const uint8_t max_bytes = 13;
    static const uint8_t repeat = 1;
    if(current_idx < max_bytes)
    {
        if(SendByte(str[current_idx]))
        {
            if(++current_idx == max_bytes)
            {
                if(repeat)
                {
                    current_idx = 0;
                }
            }
        } 
    }  
}

void ReceivedByte(uint8_t b, uint8_t errorOccured, uint8_t parityOK, uint8_t stopOK)
{
    Serial.print("Byte: ");
    Serial.print(b);
    if(!parityOK)
    {
        Serial.print(" | parity error occured");
    }

    if(!stopOK)
    {
        Serial.print(" | stop error occured");
    }
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
    
    pulseArray[pulseCount++] = digitalRead(RX);
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
            ReceivedByte(readByte, !(parityCheck && readBit), parityCheck, readBit);
            reading = 0;
        }
        ++readBitCount;
    }
}

ISR(TIMER1_COMPA_vect)
{
    ProcessSend();
}

void setup() 
{  
    Serial.begin(9600);
    
    pinMode(9, OUTPUT);
    pinMode(10, OUTPUT);
    pinMode(13, OUTPUT);
    pinMode(12, INPUT);
    
    sendBuffer.init(32);
    recvBuffer.init(32);
    PORTB |= _BV(PORTB2);
    digitalWrite(10, 1);
    delay(5000);
    /*
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
    
    // enable global interrupts
    sei();*/
}

uint8_t xxxx = 0;
void loop() 
{
    delay(10);
    ProcessSend();
    //ProcessReceive();
    /*if(xxxx < 3)
    {
        ++xxxx;
    }
    else
    {
        
    }*/
}
