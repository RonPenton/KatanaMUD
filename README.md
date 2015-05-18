#Development Server

We now have a development server. The server may not always be running (if it crashes, for example), and may be taken down frequently for updates. But when it's up, it's here: 

* http://katanamud.cloudapp.net/

#Building

Information on how to build the project can be found here: https://github.com/RonPenton/KatanaMUD/wiki/Building

# KatanaMUD
A web browser MUD. It originally started life as a clone of MajorMUD, but may end up diverging from that vision significantly in the end.

Chat with us on Jabbr: https://jabbr.net/#/rooms/KatanaMUD

At the moment, it's just me developing, but I'm open to collaborators. Fork, modify, and make a pull request!


#Status
The project is just starting development at the moment. A lot of the ideas are very new and in a lot of flux, so things may change rapidly. I'm building it on ASP.NET 5, which is currently in a pre-release beta state, and that may affect what gets developed here as the framework matures.

At some point I will set up a testing server. I need to buy a VPS to do this, because my internet connection is far too slow right now to handle running a game server. I think this will be around the 0.3 milestone. If anyone has a VPS they can lend me in the meantime that would be superb, but don't sweat it if not. 

#Why KatanaMUD? What does that have to do with MajorMUD?
There's a lot of reasons for KatanaMUD. The MajorMUD community is dying, and there's no "savior" on the horizon. There's a very good port of it out there but they are unfortunately suffering from a glacial pace of development. I wish they'd just open the source already, but it's their choice, and we must respect that. In the meantime, the waning playerbase is clamouring for more content and features, and I feel like I want to at least attempt to offer it to them. MUD's have a long history of being open sourced, and because of that history, KatanaMUD is open sourced as well. The work I put into this game will be freely available to the rest of the world long after I've moved on or croaked. Additionally, this is a tacit admission that a project of this size is more than I can chew at the moment. I will need help at some point. MUD's are complex, and take thousands of hours to complete. Hopefully the community can come together and finish this out.

Why the name "KatanaMUD"? Simple. The game started off as an experiment on running a websocket MUD using the Katana web server. Unfortunately that web server did not support the ASP.MVC view engine, so the web component of the game would be extremely difficult to program. Now that the new ASP.NET 5 platform has replaced Katana, development has moved onto that platform instead, but the name stuck. 

#Engine Design
The game has two primary components. The WebSockets game loop, and the MVC view engine. The Game Loop manages all in-game communication, all the real-time stuff, like a normal telnet server would. The MVC View Engine will be used primarily for data retrieval purposes; displaying pages, showing the top 100 players, etc. HTML offers us the ability to provide more complex UI's as well, so the character creation and stat editing process will be accomplished using HTML UI's. 

We have abandoned Telnet. RIP. It simply does not offer us the features that we want to use at this point. It's too archaic. Instead communication with the server happens via JSON message passing. That way, both the client and the server can operate using a predefined message system and not waste too much effort on complex parsing routines, like many telnet clients do these days. The server will tell the client which room the user is in, and the client will be able to take the appropriate action to notify the user. This way, the client can be slowly upgraded over time to remove some text-based components and replace them with more rich UI components.

The Game Loop runs on a single thread. Unfortunately this is probably a necessity. The game will eventually have a robust scripting engine, and something like that is just asking to be vulnerable to deadlocks. So by sticking to a single thread model, we limit the chances of that happening. One interesting wrinkle, however, is the MVC view engine, which will not be running on the game thread. I think we'll be ok there if we stick to mostly data retrieval methods in the MVC engine, but there's always a chance of data corruption if something is updated when a page is loaded. We'll have to experiment with this more to see if it's a real concern.

#Roadmap

This is a preliminary roadmap for the game. Everything is subject to change on my whims.

## 0.1
- [x] Basic Proof-Of-Concept using Websockets
- [x] Design Membership system
- [x] Design Login/Socket Management System
- [x] Design messaging system
- [x] MajorMUD Data Importer Tool from Nightmare Redux data
- [x] Design ORM Layer
- [x] Basic Map Module (rooms, normal exits)
- [x] Communication module (talk, gossip, telepaths)

## 0.2

- [ ] Item module
- [ ] Movement Limitation
- [ ] Command/Data Rate Limitations and queuing
- [ ] Admin/Log Module (All admin commands will be publicly logged, to promote honesty in operation)
- [ ] Gangs
- [ ] Communication Enhancements: chatrooms, gangpaths, officerpaths

## 0.3

- [ ] Scripting Engine
- [ ] Portals (advanced exit types)
- [ ] Stat System
- [ ] Buff System

## 0.4

- [ ] Combat
- [ ] Skill Trees (spells, skills)

## 0.5

- [ ] Import Textblock "scripts" into native format
- [ ] Quest/event module

## 0.6 

- [ ] Content Editors

## 1.0

- [ ] Balancing of the game
- [ ] Automation of client (ie: MegaMUD-like features)

## 2.0

- [ ] Factions 
- [ ] Auctions (?)
- [ ] New Content

Key: 
* *italic - Done or mostly done*
* **bold - Currently in progress**
* regular - To Do
* ~~strikethrough - Removed from roadmap~~

#Notes

I've temporarily removed Multi-tennancy from the roadmap. The idea behind this concept is that eventually people might want to run multiple games on the same server, like have one realm for PvP, one realm for PvE, and maybe add new realms periodically instead of wiping an old realm. Supporting mutli-tennancy innately would have multiple benefits, such as allowing one single server to run multiple games, making administration easier, and so forth. However, after thinking about it, I'm not really sure how likely it is that this feature would be used extensively. It's a "good idea" but may just be one of those things that you spend a lot of time on for little to no real world benefit. MUD Games already have population issues so perhaps it's not the greatest idea to encourage more servers to open and spread out the population. So I'm removing it from the roadmap for the time being, with an option to look at it again in the future. The design of the system would be to use the same websocket for all games, using the Websocket protocol to specify which server the user wants to join. Once they connect, their connection will be handed off to the correct socket manager, each of which runs its own instance of the game, attached to a separate SQL database. The reason for the separate SQL databases is because it'll just be a thousand times easier to manage, rather than having to maintain an "instance ID" on each table as well. In theory, it shouldn't be too difficult to adapt the game to use this model if we ever end up implementing it. 
