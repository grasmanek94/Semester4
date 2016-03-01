#define TX (13)
#define RX (12)

#include "ByteBuffer.h"
ByteBuffer sendBuffer;
ByteBuffer recvBuffer;

bool GetBitAt(unsigned char b, unsigned char bit)
{
  return !!(b & (1 << bit));  
}

uint8_t receiveByte()
{
    
}

void setup() 
{
  Serial.begin(9600);
  
  sendBuffer.init(32);
  recvBuffer.init(32);

  // disable interrupts
  cli();

  // clear registers
  TCCR1A = 0;
  TCCR1B = 0;

  // CTC Mode
  TCCR1B |= _BV(WGM12);

  // Prescaler 64
  TCCR1B |= _BV(CS10) | _BV(CS11);

  // Timer interrupt enable
  TIMSK1 = 0;
  TIMSK1 |= _BV(OCIE1A);

  // Button interrupt
  PCMSK0 |= _BV (PCINT3); // This is D11
  PCIFR  |= _BV (PCIF0);
  PCICR  |= _BV (PCIE0); // Enable interrupt

  // enable global interrupts
  sei();
}

void loop() 
{

}
