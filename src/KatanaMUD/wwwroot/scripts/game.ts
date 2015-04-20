/// <reference path="jquery.d.ts" />

module KMud {

    export interface Func0<R> { (): R; }
    export interface Func1<P, R> { (parameter: P): R; }
    export interface Func2<P1, P2, R> { (parameter1: P1, parameter2: P2): R; }
    export interface Func3<P1, P2, P3, R> { (parameter1: P1, parameter2: P2, parameter3: P3): R; }
    export interface Func4<P1, P2, P3, P4, R> { (parameter1: P1, parameter2: P2, parameter3: P3, parameter4: P4): R; }
    export interface Action0 { (): void; }
    export interface Action1<P> { (parameter: P): void; }
    export interface Action2<P1, P2> { (parameter1: P1, parameter2: P2): void; }
    export interface Action3<P1, P2, P3> { (parameter1: P1, parameter2: P2, parameter3: P3): void; }
    export interface Action4<P1, P2, P3, P4> { (parameter1: P1, parameter2: P2, parameter3: P3, parameter4: P4): void; }

    export interface Dictionary<T> { [index: string]: T }



    export class Game {
        private input: HTMLInputElement;
        private lastCommands: string[] = [];
        private currentCommand: number = -1;
        private messageHandlers: { [index: string]: Action1<MessageBase> } = {};
        private commandHandlers: { [index: string]: Action3<string[], string, string> } = {};
        private symbolCommandHandlers: { [index: string]: Action3<string[], string, string> } = {};

        constructor() {
            this.input = <HTMLInputElement>document.getElementById("InputBox");
            $("#InputBox").keypress(x => {
                if (x.keyCode == 13 && this.input.value.trim() != "") {
                    this.addOutput(document.getElementById("Output"), this.input.value, "command-text");
                    this.lastCommands.unshift(this.input.value);
                    this.processCommand(this.input.value);
                    this.input.value = "";
                    this.currentCommand = -1;

                    while (this.lastCommands.length > 20) {
                        this.lastCommands.shift()
                    }
                }
            }).keydown(x => {
                if (x.keyCode == 38 && x.ctrlKey) {
                    if (this.currentCommand < this.lastCommands.length - 1) {
                        this.currentCommand++;
                        this.input.value = this.lastCommands[this.currentCommand];
                    }
                }
                else if (x.keyCode == 40 && x.ctrlKey) {
                    if (this.currentCommand >= 1) {
                        this.currentCommand--;
                        this.input.value = this.lastCommands[this.currentCommand];
                    }
                    else if (this.currentCommand == 0) {
                        this.input.value = "";
                        this.currentCommand = -1;
                    }
                }

            });
            $(window).mouseup(x => {
                if (window.getSelection().toString().length == 0)
                    $("#InputBox").focus();
            });

            this.registerMessageHandlers();
            this.registerCommandHandlers();
        }

        private processCommand(command: string) {
            var lower = command.toLocaleLowerCase().trim();
            var words = command.split(/\s+/gi);
            var tail = command.substr(words[0].length).trim();
            var param = words[0].substr(1);

            var handler = this.commandHandlers[words[0]];
            if (handler) {
                handler(words, tail, words.length > 1 ? words[1] : null);
                return;
            }

            // Check for character commands; ie "/Mithrandir hey there"
            var char = words[0].substr(0, 1);
            var charHandler = this.symbolCommandHandlers[char]
            if (charHandler != null) {
                charHandler(words, tail, param);
                return;
            }

            // No clue. Say it, don't spray it.
            this.talk(command, CommunicationType.Say);
        }

        private _socket: WebSocket;

        public connect(url: string) {
            this._socket = new WebSocket(url, "kmud");
            this._socket.onopen = e => this.onopen(e);
            this._socket.onclose = e => this.onclose(e);
            this._socket.onerror = e => this.onerror(e);
            this._socket.onmessage = e => this.onmessage(e);
        }

        public SendMessage(message: MessageBase) {
            if (this._socket) {
                this._socket.send(JSON.stringify(message));
            }
        }

        private onopen(e: Event) {

        }
        private onclose(e: Event) {
            this.addOutput(document.getElementById("Output"), "Disconnected", "error-text");
        }
        private onerror(e: Event) {
        }
        private onmessage(e: Event) {
            var message: MessageBase = JSON.parse((<any>e).data);

            var handler = this.messageHandlers[message.MessageName];
            if (handler != null) {
                handler(message);
            }
        }

        private registerMessageHandlers() {
            this.messageHandlers[LoginRejected.ClassName] = (message: LoginRejected) => {
                this.addOutput(document.getElementById("Output"), message.RejectionMessage, "error-text");
                if (message.NoCharacter == true) {
                    window.location.replace("/Home/ChooseRace");
                }
            }

            this.messageHandlers[ServerMessage.ClassName] = (message: ServerMessage) => {
                this.addOutput(document.getElementById("Output"), message.Contents);
            }

            this.messageHandlers[PongMessage.ClassName] = (message: PongMessage) => {
                var latency = new Date().valueOf() - new Date((<any>message).SendTime).valueOf();
                this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            }

            this.messageHandlers[RoomDescriptionMessage.ClassName] = (message: RoomDescriptionMessage) => this.showRoomDescription(message);

            this.messageHandlers[CommunicationMessage.ClassName] = (message: CommunicationMessage) => this.showCommunication(message);
        }

        private registerCommandHandlers() {
            this.commandHandlers["ping"] = x => this.ping();

            this.commandHandlers["n"] = this.commandHandlers["north"] = words => this.move(Direction.North);
            this.commandHandlers["s"] = this.commandHandlers["south"] = words => this.move(Direction.South);
            this.commandHandlers["e"] = this.commandHandlers["east"] = words => this.move(Direction.East);
            this.commandHandlers["w"] = this.commandHandlers["west"] = words => this.move(Direction.West);
            this.commandHandlers["ne"] = this.commandHandlers["northeast"] = words => this.move(Direction.Northeast);
            this.commandHandlers["nw"] = this.commandHandlers["northwest"] = words => this.move(Direction.Northwest);
            this.commandHandlers["se"] = this.commandHandlers["southeast"] = words => this.move(Direction.Southeast);
            this.commandHandlers["sw"] = this.commandHandlers["southwest"] = words => this.move(Direction.Southwest);
            this.commandHandlers["u"] = this.commandHandlers["up"] = words => this.move(Direction.Up);
            this.commandHandlers["d"] = this.commandHandlers["down"] = words => this.move(Direction.Down);
            this.commandHandlers["l"] = this.commandHandlers["look"] = words => this.look(words);

            this.commandHandlers["gos"] = this.commandHandlers["gossip"] = (words, tail) => this.talk(tail, CommunicationType.Gossip);
            this.commandHandlers["say"] = (words, tail) => this.talk(tail, CommunicationType.Say);
            this.commandHandlers["telepath"] = (words, tail, param) => this.talk(tail, CommunicationType.Telepath, param);

            this.symbolCommandHandlers["."] = (words, tail) => this.talk(tail, CommunicationType.Gossip);
            this.symbolCommandHandlers["/"] = (words, tail, param) => this.talk(tail, CommunicationType.Telepath, param);
        }

        private talk(text: string, type: CommunicationType, param?: string) {
            var message = new CommunicationMessage();
            message.Message = text;
            message.Type = type;
            if (param !== undefined) {
                message.ActorName = param;
            }
            this._socket.send(JSON.stringify(message));
        }

        private look(words: string[]) {
            var message = new LookMessage()
            //TODO: Parse parameters
            this._socket.send(JSON.stringify(message));
        }

        private move(direction: Direction) {
            var message = new MoveMessage()
            message.Direction = direction;
            this._socket.send(JSON.stringify(message));
        }

        private ping() {
            var message = new PingMessage();
            message.SendTime = new Date();
            this._socket.send(JSON.stringify(message));
        }

        private addOutput(element: HTMLElement, text: string, css: string = null) {

            var scrolledToBottom = (element.scrollHeight - element.scrollTop === element.offsetHeight);

            var span = document.createElement("span");
            span.textContent = text;
            span.className = css;
            element.appendChild(span);
            var br = document.createElement("br");
            element.appendChild(br);

            // Keep scrolling to bottom if they're already there.
            if (scrolledToBottom) {
                element.scrollTop = element.scrollHeight;
            }
        }

        private mainOutput(text: string, css: string = null) {
            this.addOutput(document.getElementById("Output"), text, css);
        }

        private showRoomDescription(message: RoomDescriptionMessage) {
            if (message.CannotSee) {
                this.mainOutput(message.CannotSeeMessage, "cannot-see");
            }
            else {
                this.mainOutput(message.Name, "room-name");
                if (StringUtilities.notEmpty(message.Description)) {
                    this.mainOutput(message.Description, "room-desc");
                }

                if (message.Actors.length > 0) {
                    this.mainOutput("Also here: " + message.Actors.map(x=> x.Name).join(", ") + ".", "actors");
                }

                if (message.Exits.length > 0) {
                    this.mainOutput("Obvious exits: " + message.Exits.map(x=> x.Name).join(", "), "exits");
                }
                else {
                    this.mainOutput("Obvious exits: NONE!!!", "exits");
                }
            }
        }

        private showCommunication(message: CommunicationMessage) {
            switch (message.Type) {
                case CommunicationType.Gossip:
                    this.mainOutput(message.ActorName + " gossips: " + message.Message, "gossip");
                    break;

                case CommunicationType.Say:
                    this.mainOutput(message.ActorName + " says \"" + message.Message + "\"", "say");
                    break;

                case CommunicationType.Telepath:
                    this.mainOutput(message.ActorName + " telepaths " + message.Message, "telepath");
                    break;
            }
        }
    }

    export class StringUtilities {
        public static notEmpty(s: string): boolean {
            return !this.isNullOrWhitespace(s);
        }

        public static isNullOrEmpty(s: string): boolean {
            return (s === null || s === undefined || s === "");
        }

        public static isNullOrWhitespace(s: string): boolean {
            return (s === null || s === undefined || /^\s*$/g.test(s));
        }

        public static formatString(str: string, args: any[]): string {
            return str.replace(/{(\d+)}/g, function (match: string, ...i: any[]) {
                return typeof args[i[0]] != 'undefined' ? args[i[0]] : match;
            });
        }


        // Referring to the table here:
        // https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/regexp
        // these characters should be escaped
        // \ ^ $ * + ? . ( ) | { } [ ]
        // These characters only have special meaning inside of brackets
        // they do not need to be escaped, but they MAY be escaped
        // without any adverse effects (to the best of my knowledge and casual testing)
        // : ! , = 
        private static _escapeRegExpCharacters: string[] = [
            "-", "[", "]",  // order matters for these
            "/", "{", "}", "(", ")", "*", "+", "?", ".", "\\", "^", "$", "|" // order doesn't matter for any of these
        ];

        private static _escapeRegExpRegexp: RegExp = new RegExp('[' + StringUtilities._escapeRegExpCharacters.join('\\') + ']', 'g');

        public static escapeRegExp(str: string): string {
            return str.replace(StringUtilities._escapeRegExpRegexp, "\\$&");
        }
    }

}