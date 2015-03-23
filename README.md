# KatanaMUD
A web browser MUD. It originally started life as a clone of MajorMUD, but may end up diverging from that vision significantly in the end. Who knows. We shall see!


#Status
The project is just starting development at the moment. A lot of the ideas are very new and in a lot of flux, so things may change rapidly. I'm building it on ASP.NET 5, which is currently in a pre-release beta state, and that may affect what gets developed here as the framework matures.

#Limitations
Due to requiring Websockets, The MUD will not run (in its current state) on any Windows OS before Windows 8. This is due to a limitation in the IIS/HttpListener servers which rely on HTTP.sys, which does not support Websockets pre-W8. Now, there may be a way around this by using the Kestrel web server instead, but I am currently unable to get that working, and frankly it's not worth my time fighting since there's no documentation about it anywhere. If anyone wants to lend a hand and get that working, please do.

#Roadmap

* (current) Basic Proof-Of-Concept using Websockets
* Design multi-tennancy system (to run multiple MUDs on the same server)
* Get Membership working with multi-tennancy
* Design messaging system (Typescript code-gen of message classes?)
* Design world module
* Design communication module
* Design Admin/Log Module (All admin commands will be publicly logged, to promote honesty in operation)
* Design item module
* Design combat module
* Design spell module
* Design quest/event module
* (?) MajorMUD Data Importer Tool from Nightmare Redux data
* Web-based content editor.
* (?) Design Customization Module, to allow for altering the game without the need for rebooting/resetting.
* (?) Look into expanding the features of the game. Factions, Auctions, etc.
* Automation of client (ie: MegaMUD-like features)
