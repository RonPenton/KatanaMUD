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
    export class LookMessage extends MessageBase {
        constructor() { super('LookMessage'); }
        public Direction: Direction;
        public Actor: number;
        public Item: number;
        public YouFigureItOut: string;
        public static ClassName: string = 'LookMessage';
    }
    export class MoveMessage extends MessageBase {
        constructor() { super('MoveMessage'); }
        public Direction: Direction;
        public Portal: number;
        public static ClassName: string = 'MoveMessage';
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
    export class RoomDescriptionMessage extends MessageBase {
        constructor() { super('RoomDescriptionMessage'); }
        public RoomId: number;
        public Name: string;
        public Description: string;
        public Actors: number[];
        public VisibleItems: number[];
        public Exits: number[];
        public IsCurrentRoom: boolean;
        public CannotSee: boolean;
        public CannotSeeMessage: string;
        public static ClassName: string = 'RoomDescriptionMessage';
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
    export enum Direction {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
        Northeast = 4,
        Northwest = 5,
        Southeast = 6,
        Southwest = 7,
        Up = 8,
        Down = 9,
    }
}
