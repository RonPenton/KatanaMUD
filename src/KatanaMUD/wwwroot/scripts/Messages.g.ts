module KMud {
    export class LoginRejected extends MessageBase {
        constructor() { super('LoginRejected'); }
        public RejectionMessage: string;
        public NoCharacter: boolean;
        public static ClassName: string = 'LoginRejected';
    }
    export class PingMessage extends MessageBase {
        constructor() { super('PingMessage'); }
        public SendTime: Date;
        public static ClassName: string = 'PingMessage';
    }
    export class PongMessage extends MessageBase {
        constructor() { super('PongMessage'); }
        public SendTime: Date;
        public static ClassName: string = 'PongMessage';
    }
    export class ServerMessage extends MessageBase {
        constructor() { super('ServerMessage'); }
        public Contents: string;
        public static ClassName: string = 'ServerMessage';
    }
}
