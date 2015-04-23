module KMud {
    export class ActorInformationMessage extends MessageBase {
        constructor() { super('ActorInformationMessage'); }
        public Name: string;
        public Id: string;
        public IsYou: boolean;
        public static ClassName: string = 'ActorInformationMessage';
    }
    export class CommunicationMessage extends MessageBase {
        constructor() { super('CommunicationMessage'); }
        public Message: string;
        public Type: CommunicationType;
        public Chatroom: string;
        public ActorName: string;
        public ActorId: string;
        public static ClassName: string = 'CommunicationMessage';
    }
    export class InventoryMessage extends MessageBase {
        constructor() { super('InventoryMessage'); }
        public static ClassName: string = 'InventoryMessage';
    }
    export class InventoryListMessage extends MessageBase {
        constructor() { super('InventoryListMessage'); }
        public Items: ItemDescription[];
        public Encumbrance: number;
        public MaxEncumbrance: number;
        public static ClassName: string = 'InventoryListMessage';
    }
    export class GetItemMessage extends MessageBase {
        constructor() { super('GetItemMessage'); }
        public ItemId: string;
        public ItemName: string;
        public Quantity: number;
        public static ClassName: string = 'GetItemMessage';
    }
    export class DropItemMessage extends MessageBase {
        constructor() { super('DropItemMessage'); }
        public ItemId: string;
        public ItemName: string;
        public Quantity: number;
        public static ClassName: string = 'DropItemMessage';
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
        public Actors: ActorDescription[];
        public VisibleItems: ItemDescription[];
        public Exits: ExitDescription[];
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
    export class ItemDescription {
        public Name: string;
        public Id: string;
        public TemplateId: number;
    }
    export class ActorDescription {
        public Name: string;
        public Id: string;
    }
    export class ExitDescription {
        public Direction: Direction;
        public Name: string;
        public DestinationRoom: number;
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
