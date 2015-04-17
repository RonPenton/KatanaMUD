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
        private commandHandlers: { [index: string]: Action1<MessageBase> } = {};

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

            this.registerHandlers();
        }

        private processCommand(command: string) {
            var lower = command.toLocaleLowerCase().trim();
            var words = command.split(/\s+/gi);
            if (words[0] == "ping") {
                var message = new PingMessage();
                message.SendTime = new Date();
                this._socket.send(JSON.stringify(message));
            }
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

            var handler = this.commandHandlers[message.MessageName];
            if (handler != null) {
                handler(message);
            }
        }

        private registerHandlers() {
            this.commandHandlers[LoginRejected.ClassName] = (message: LoginRejected) => {
                this.addOutput(document.getElementById("Output"), message.RejectionMessage, "error-text");
                if (message.NoCharacter == true) {
                    window.location.replace("/Home/ChooseRace");
                }
            }

            this.commandHandlers[ServerMessage.ClassName] = (message: ServerMessage) => {
                this.addOutput(document.getElementById("Output"), message.Contents);
            }

            this.commandHandlers[PongMessage.ClassName] = (message: PongMessage) => {
                var latency = new Date().valueOf() - new Date((<any>message).SendTime).valueOf();
                this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            }

            this.commandHandlers[RoomDescriptionMessage.ClassName] = (message: RoomDescriptionMessage) => this.showRoomDescription(message);
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