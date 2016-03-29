unsigned long send_packets = 0;
unsigned long received_packets = 0;
char buffer[32];

bool sending = false;
bool receiving = false;

// change '+ X' for every arduino:
const char MY_ARDUINO_ID = 'A' + 0;

/*
	'T'
	SOURCE ADDRES
	AMOUNT_OF_BYTES_AFTER_THIS
	PACKET_NUM
	DATA
*/

void setup()
{
	Serial.begin(9600);
}

void loop()
{
	if (!sending)
	{
		if (Serial.available())
		{
			if (Serial.read() == 'T')
			{
				long start_time = millis();
				while (!Serial.available() && millis() - start_time < 25);
				if (Serial.available() && Serial.read() != MY_ARDUINO_ID)
				{
					unsigned char bytesAmount = Serial.read();
					unsigned long received_packet_number;
					start_time = millis();
					while (bytesAmount > 0 && bytesAmount < 32 && Serial.available() < bytesAmount && millis() - start_time < 25);
					if (bytesAmount > 0 && bytesAmount < 32 && bytesAmount >= sizeof(received_packet_number))
					{
						Serial.readBytes((uint8_t*)&received_packet_number, sizeof(received_packet_number));
						if (bytesAmount - sizeof(received_packet_number) > 0)
						{
							Serial.readBytes((uint8_t*)buffer, bytesAmount - sizeof(received_packet_number));
							if (buffer[0] == 'H' && buffer[11] == '!' && buffer[12] == 0)
							{
								++received_packets;
							}
						}
					}
				}
			}
		}
		else if (Serial.available() == 0)
		{
			sending = true;
			Serial.write('T');
			Serial.write(MY_ARDUINO_ID);
		}	
	}
	else if(sending)
	{
		Serial.write((char)((int)log10(send_packets) + 1 + sizeof("Hello World!\r\n")));
		Serial.print(send_packets);
		Serial.print("Hello World!\r\n");
		Serial.flush();
		send_packets++;
		sending = false;
	}
}
/*
Gemeten stats: gem. packets per second met 2 arduino's op 1 lijn: ~250 (ofwel ~4.88 kib/s met de gebruikte data)
Externe serial ontvanger gebruikt (PC) en gekeken naar packet number en tijden of arrival
*/