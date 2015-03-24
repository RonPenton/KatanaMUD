# KatanaMUD
A web browser MUD. It originally started life as a clone of MajorMUD, but may end up diverging from that vision significantly in the end. Who knows. We shall see!


#Status
The project is just starting development at the moment. A lot of the ideas are very new and in a lot of flux, so things may change rapidly. I'm building it on ASP.NET 5, which is currently in a pre-release beta state, and that may affect what gets developed here as the framework matures.

#Limitations
Due to requiring Websockets, The MUD will not run (in its current state) on any Windows OS before Windows 8. This is due to a limitation in the IIS/HttpListener servers which rely on HTTP.sys, which does not support Websockets pre-W8. Now, there may be a way around this by using the Kestrel web server instead, but I am currently unable to get that working, and frankly it's not worth my time fighting since there's no documentation about it anywhere. If anyone wants to lend a hand and get that working, please do.

#Roadmap

* (current) Basic Proof-Of-Concept using Websockets
* Design Membership system
* Design Login/Socket Management System
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
* ~~Design multi-tennancy system (to run multiple MUDs on the same server)~~
* 
#Notes

I've temporarily removed Multi-tennancy from the roadmap. The idea behind this concept is that eventually people might want to run multiple games on the same server, like have one realm for PvP, one realm for PvE, and maybe add new realms periodically instead of wiping an old realm. Supporting mutli-tennancy innately would have multiple benefits, such as allowing one single server to run multiple games, making administration easier, and so forth. However, after thinking about it, I'm not really sure how likely it is that this feature would be used extensively. It's a "good idea" but may just be one of those things that you spend a lot of time on for little to no real world benefit. MUD Games already have population issues so perhaps it's not the greatest idea to encourage more servers to open and spread out the population. So I'm removing it from the roadmap for the time being, with an option to look at it again in the future. The design of the system would be to use the same websocket for all games, using the Websocket protocol to specify which server the user wants to join. Once they connect, their connection will be handed off to the correct socket manager, each of which runs its own instance of the game, attached to a separate SQL database. The reason for the separate SQL databases is because it'll just be a thousand times easier to manage, rather than having to maintain an "instance ID" on each table as well. In theory, it shouldn't be too difficult to adapt the game to use this model if we ever end up implementing it. 
