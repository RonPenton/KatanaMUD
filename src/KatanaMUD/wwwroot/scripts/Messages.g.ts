module KMud {
    export class CommunicationMessage extends MessageBase {
        constructor() { super('CommunicationMessage'); }
        public Message: string;
        public Type: CommunicationType;
        public Chatroom: string;
        public User: number;
        public static ClassName: string = 'CommunicationMessage';
    }
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
    export enum CommunicationType {
        Gossip = 0,
        Auction = 1,
        Say = 2,
        Yell = 3,
        Region = 4,
        Gangpath = 5,
        Officerpath = 6,
        Chatroom = 7,
        Telepath = 8,
    }
}
