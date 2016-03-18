#include <ESP8266WiFi.h>

extern volatile uint32_t PIN_OUT;
extern volatile uint32_t PIN_OUT_SET;
extern volatile uint32_t PIN_OUT_CLEAR;

extern volatile uint32_t PIN_DIR;
extern volatile uint32_t PIN_DIR_OUTPUT;
extern volatile uint32_t PIN_DIR_INPUT;

extern volatile uint32_t PIN_IN;

extern volatile uint32_t PIN_0;
extern volatile uint32_t PIN_2;

class WifiCredentials
{
    String _ssid;
    String _password;
    
public:
    WifiCredentials(const String ssid, const String password)
    : _ssid(ssid), _password(password){ }

    const String GetSSID() { return _ssid; }
    const String GetPASSWD() { return _password; }
};

const int MAX_SSID = 2;
WifiCredentials ssid[MAX_SSID] =
{
    WifiCredentials("H369AC2012E", "A9AD4936CAF6"),
    WifiCredentials("GZX-2.4G", "1234567890abcdef")
};

const long SSID_TIMEOUT = 30000;
long SSID_CONNECT_START = 0;

bool GetGPIO(int gpio_number)
{
    return PIN_IN & (1 << gpio_number);
}

void SetGPIO(int gpio_number, bool state)
{
    PIN_OUT ^= (-state ^ PIN_OUT) & (1 << gpio_number);
}

const char* host = "ip.gz0.nl";

int PIN_BLUE = D6;
int GPIO_BLUE = 12;

int PIN_RED = D3;
int GPIO_RED = 5;

int PIN_YELLOW = D4;
int GPIO_YELLOW = 4;

int PIN_GREEN = D5;
int GPIO_GREEN = 14;

int GPIO_NUM[MAX_SSID] =
{
    GPIO_GREEN,
    GPIO_YELLOW    
};

int GPIO_STATE[MAX_SSID] =
{
    0,
    0
};

bool connected_wifi = false;
bool connected_internet = false;

int ScanWifi()
{
    WiFi.disconnect();  
    WiFi.mode(WIFI_STA);
 
}

bool ConnectWifi()
{
    SetGPIO(GPIO_RED, 0);
    SetGPIO(GPIO_GREEN, 0);
    SetGPIO(GPIO_BLUE, 0);
    SetGPIO(GPIO_YELLOW, 0);
 
    connected_internet = false;
    connected_wifi = false;
    
    for(int i = 0; i < MAX_SSID; ++i)
    {        
        WiFi.disconnect();
        WiFi.mode(WIFI_AP);
        WiFi.begin(ssid[i].GetSSID().c_str(), ssid[i].GetPASSWD().c_str());
        
        SSID_CONNECT_START = millis() + SSID_TIMEOUT;
        while (WiFi.status() != WL_CONNECTED && millis() < SSID_CONNECT_START) 
        {
            delay(250);
            SetGPIO(GPIO_NUM[i], GetGPIO(GPIO_NUM[i]) ^ 1);             
        }
        
        if(WiFi.status() != WL_CONNECTED)
        {
            SetGPIO(GPIO_NUM[i], 0);
        }
        else
        {
            connected_wifi = true;
            SetGPIO(GPIO_RED, 0);
            SetGPIO(GPIO_NUM[i], 1);
            return true;
        }
    }

    connected_wifi = false;
    return false;
}

bool ConnectInternet()
{
    connected_internet = false;
    WiFiClient client;
    if (client.connect(host, 80)) 
    {
        // This will send the request to the server
        client.print(String("GET / HTTP/1.1\r\nHost: ip.gz0.nl\r\nConnection: close\r\n\r\n"));
        SSID_CONNECT_START = millis() + 5000;
        while (client.available() == 0) 
        {
            delay(75);
            SetGPIO(GPIO_BLUE, GetGPIO(GPIO_BLUE) ^ 1);
            SetGPIO(GPIO_RED, GetGPIO(GPIO_BLUE) ^ 1);
            if (millis() > SSID_CONNECT_START) 
            {                              
                client.stop();
                break;
            }
        }
      
        while(client.available())
        {          
            String line = client.readStringUntil('\r');
            if(line.indexOf("77.169.137.254") != -1)
            {
                connected_internet = true;
            }
        }
    }  

    return connected_internet;
}

void setup() 
{
    pinMode(PIN_BLUE, OUTPUT);
    pinMode(PIN_RED, OUTPUT);
    pinMode(PIN_YELLOW, OUTPUT);
    pinMode(PIN_GREEN, OUTPUT);
   
    Serial.begin(9600);
    delay(10);
}

void loop() 
{    
    if(!connected_wifi)
    {
        ConnectWifi();
    }
    else
    {
        delay(connected_internet ? 10000 : 1000);
        if(!ConnectInternet())
        {
            connected_wifi = false;
        }
    }
    
    SetGPIO(GPIO_RED, !connected_internet && !connected_wifi);
    SetGPIO(GPIO_BLUE, connected_internet);   
}
