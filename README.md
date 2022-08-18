# Amazing Chat Bot
This is a simple chat bot that has two main functionalities: Genshin Impact style gacha minigame with persistent loot and unit converter

**Gacha minigame**
It's activated with channel point redemption and also has 2 commands to check what user has dropped:
- *!check [item name]*
	- for example *!check timmie* -  it will display how many times calling user received that item
- *!checkdrops*
	- it will display total drops for user calling it

All possible drops in the minigame can be configured in file *gachaMinigame1Config.json*
All user drops are stored in *gachaMinigame1SavesDrops.json*

---

**Unit Converter**
It's activated by people just typing specific values into chat, bot will automatically detect them and display all conversions in single message.
All supported units are:
- Feet: **ft** or **'** -> meters
	- 5ft = *Conversion: 5 feet = 1.52m.*
	- 5' = *Conversion: 5 feet = 1.52m.*
- Inches: **in** or **"** -> millimeters
	- 5in = *Conversion: 5 inches = 127.20mm.*
	- 5" = *Conversion: 5 inches = 127.20mm.*
- Feet and inches: **ft** or **'** and **in** or **"** -> meters
	- 5ft 5in = *Conversion: 5ft 5in = 1.65m.*
	- 5ft 5" = *Conversion: 5ft 5in = 1.65m.*
	- 5' 5in = *Conversion: 5ft 5in = 1.65m.*
	- 5' 5" = *Conversion: 5ft 5in = 1.65m.*
- Miles: **miles** and **mi** -> kilometers
	- 5mi = *Conversion: 5 miles = 8.05km.*
- Fahrenheit: **f** -> Celsius
	- 50f = *Conversion: 50°F = 10°C.*
- Celsius: **c** -> Fahrenheit
	- 50c = *Conversion: 50°C = 122°F.*
- Kilometers: **km** or **kilometers** -> miles
	- 5km = *Conversion: 5km = 3.11 miles.*
	- 5 kilometers = *Conversion: 5km = 3.11 miles.*
- Meters: **m** or **meters** -> inches
	- 5m = *Conversion: 5m = 16.40ft.*
	- 5 meters = *Conversion: 5m = 16.40ft.*



## Setup:
Everything is configured via config.json file

- channelName
	-  name of the channel bot will connect to
- channelId
	-  ID of the channel, if you are not planning to use the Gacha Minigame you can ignore this, to find out your channel id you can use this browser extension: [link](https://chrome.google.com/webstore/detail/twitch-username-and-user/laonpoebfalkjijglbjbnkfndibbcoon)
- botAccountName
	-  name of the bot account
- botAuthToken
	-  oauth to be able to connect, to get it follow this URL [link](https://twitchapps.com/tmi/) and login with your bot account
- botAccessToken 
	- token that's says what permissions does the bot have, to generate it follow this URL [link](https://twitchtokengenerator.com/), login with your bot account and select permissions *user_read*, *chat_login* and *channel:read:redemptions*, you can skip the last one if you are not planning on using the Gacha Minigame
- gachaMinigame1Pull1Name 
	- the name of the Channel Point Redemption Reward for single pull, can be ignored if not using the minigame
- gachaMinigame1Pull10Name
	- the name of the Channel Point Redemption Reward for single pull, can be ignored if not using the minigame
- gachaMinigame1Enabled
	- controls whether the minigame is enabled
- unitConverterEnabled
	- controls whether the unit converter is enabled
- unitConverterFeetEnabled
	- controls whether **feet** and **feet and inches** converter is enabled
- unitConverterInchesEnabled
	- controls whether **inches** converter is enabled 
- unitConverterFahrenheitEnabled
	- controls whether **Fahrenheit** converter is enabled 
- unitConverterCelsiusEnabled
	- controls whether **Celsius** converter is enabled 
- unitConverterKilometersEnabled
	- controls whether **kilometers** converter is enabled 
- unitConverterMetersEnabled
	- controls whether **meters** converter is enabled
