module KMud {
    export class ActionNotAllowedMessage extends MessageBase {
        constructor() { super('ActionNotAllowedMessage'); }
        public Message: string;
        public static ClassName: string = 'ActionNotAllowedMessage';
    }
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
    export class GenericMessage extends MessageBase {
        constructor() { super('GenericMessage'); }
        public Message: string;
        public Class: string;
        public static ClassName: string = 'GenericMessage';
    }
    export class InventoryCommand extends MessageBase {
        constructor() { super('InventoryCommand'); }
        public static ClassName: string = 'InventoryCommand';
    }
    export class InventoryListMessage extends MessageBase {
        constructor() { super('InventoryListMessage'); }
        public Cash: CurrencyDescription[];
        public Items: ItemDescription[];
        public Encumbrance: number;
        public MaxEncumbrance: number;
        public static ClassName: string = 'InventoryListMessage';
    }
    export class GetItemCommand extends MessageBase {
        constructor() { super('GetItemCommand'); }
        public ItemId: string;
        public ItemName: string;
        public Quantity: number;
        public static ClassName: string = 'GetItemCommand';
    }
    export class DropItemCommand extends MessageBase {
        constructor() { super('DropItemCommand'); }
        public ItemId: string;
        public ItemName: string;
        public Quantity: number;
        public Hide: boolean;
        public static ClassName: string = 'DropItemCommand';
    }
    export class ItemOwnershipMessage extends MessageBase {
        constructor() { super('ItemOwnershipMessage'); }
        public Items: ItemDescription[];
        public Giver: ActorDescription;
        public Taker: ActorDescription;
        public Hide: boolean;
        public static ClassName: string = 'ItemOwnershipMessage';
    }
    export class CashTransferMessage extends MessageBase {
        constructor() { super('CashTransferMessage'); }
        public Currency: CurrencyDescription;
        public Quantity: number;
        public Giver: ActorDescription;
        public Taker: ActorDescription;
        public Hide: boolean;
        public static ClassName: string = 'CashTransferMessage';
    }
    export class LoginRejected extends MessageBase {
        constructor() { super('LoginRejected'); }
        public RejectionMessage: string;
        public NoCharacter: boolean;
        public static ClassName: string = 'LoginRejected';
    }
    export class LoginStateMessage extends MessageBase {
        constructor() { super('LoginStateMessage'); }
        public Actor: ActorDescription;
        public Login: boolean;
        public static ClassName: string = 'LoginStateMessage';
    }
    export class LookMessage extends MessageBase {
        constructor() { super('LookMessage'); }
        public Direction: Direction;
        public Actor: number;
        public Item: number;
        public YouFigureItOut: string;
        public Brief: boolean;
        public static ClassName: string = 'LookMessage';
    }
    export class MoveMessage extends MessageBase {
        constructor() { super('MoveMessage'); }
        public Direction: Direction;
        public Portal: number;
        public static ClassName: string = 'MoveMessage';
    }
    export class PartyMovementMessage extends MessageBase {
        constructor() { super('PartyMovementMessage'); }
        public Leader: ActorDescription;
        public Actors: ActorDescription[];
        public Direction: Direction;
        public Enter: boolean;
        public CustomText: string;
        public static ClassName: string = 'PartyMovementMessage';
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
        public FoundItems: ItemDescription[];
        public VisibleCash: CurrencyDescription[];
        public FoundCash: CurrencyDescription[];
        public Exits: ExitDescription[];
        public IsCurrentRoom: boolean;
        public CannotSee: boolean;
        public CannotSeeMessage: string;
        public LightLevel: LightLevel;
        public static ClassName: string = 'RoomDescriptionMessage';
    }
    export class SearchCommand extends MessageBase {
        constructor() { super('SearchCommand'); }
        public Direction: Direction;
        public static ClassName: string = 'SearchCommand';
    }
    export class SearchMessage extends MessageBase {
        constructor() { super('SearchMessage'); }
        public FoundItems: ItemDescription[];
        public FoundCash: CurrencyDescription[];
        public static ClassName: string = 'SearchMessage';
    }
    export class ServerMessage extends MessageBase {
        constructor() { super('ServerMessage'); }
        public Contents: string;
        public static ClassName: string = 'ServerMessage';
    }
    export class SysopMessage extends MessageBase {
        constructor() { super('SysopMessage'); }
        public Command: string;
        public static ClassName: string = 'SysopMessage';
    }
    export class CurrencyDescription {
        public Name: string;
        public Amount: number;
    }
    export class ItemDescription {
        public Name: string;
        public Id: string;
        public TemplateId: number;
        public Modified: boolean;
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
    export enum LightLevel {
        Daylight = 50,
        Nothing = -10000,
        PitchBlack = -500,
        VeryDark = -250,
        BarelyVisible = -200,
        DimlyLit = -150,
        RegularLight = -50,
    }
}
