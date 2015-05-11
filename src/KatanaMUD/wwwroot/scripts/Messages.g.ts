module KMud {
    export class ActionNotAllowedMessage extends MessageBase {
        constructor(public Message?: string) { super('ActionNotAllowedMessage'); }
        public static ClassName: string = 'ActionNotAllowedMessage';
    }
    export class ActorInformationMessage extends MessageBase {
        constructor(public Name?: string, public Id?: string, public IsYou?: boolean) { super('ActorInformationMessage'); }
        public static ClassName: string = 'ActorInformationMessage';
    }
    export class AmbiguousItemMessage extends MessageBase {
        constructor(public Items?: ItemDescription[]) { super('AmbiguousItemMessage'); }
        public static ClassName: string = 'AmbiguousItemMessage';
    }
    export class AmbiguousActorMessage extends MessageBase {
        constructor(public Actors?: ActorDescription[]) { super('AmbiguousActorMessage'); }
        public static ClassName: string = 'AmbiguousActorMessage';
    }
    export class CommunicationMessage extends MessageBase {
        constructor(public Message?: string, public Type?: CommunicationType, public Chatroom?: string, public ActorName?: string, public ActorId?: string) { super('CommunicationMessage'); }
        public static ClassName: string = 'CommunicationMessage';
    }
    export class RemoveCommand extends MessageBase {
        constructor(public ItemId?: string, public ItemName?: string) { super('RemoveCommand'); }
        public static ClassName: string = 'RemoveCommand';
    }
    export class EquipCommand extends MessageBase {
        constructor(public ItemId?: string, public ItemName?: string) { super('EquipCommand'); }
        public static ClassName: string = 'EquipCommand';
    }
    export class ItemEquippedChangedMessage extends MessageBase {
        constructor(public Actor?: ActorDescription, public Item?: ItemDescription, public Equipped?: boolean) { super('ItemEquippedChangedMessage'); }
        public static ClassName: string = 'ItemEquippedChangedMessage';
    }
    export class GenericMessage extends MessageBase {
        constructor(public Message?: string, public Class?: string) { super('GenericMessage'); }
        public static ClassName: string = 'GenericMessage';
    }
    export class InventoryCommand extends MessageBase {
        constructor() { super('InventoryCommand'); }
        public static ClassName: string = 'InventoryCommand';
    }
    export class InventoryListMessage extends MessageBase {
        constructor(public Cash?: CurrencyDescription[], public TotalCash?: CurrencyDescription, public Items?: ItemDescription[], public Encumbrance?: number, public MaxEncumbrance?: number, public Currency?: number) { super('InventoryListMessage'); }
        public static ClassName: string = 'InventoryListMessage';
    }
    export class GetItemCommand extends MessageBase {
        constructor(public ItemId?: string, public ItemName?: string, public Quantity?: number) { super('GetItemCommand'); }
        public static ClassName: string = 'GetItemCommand';
    }
    export class DropItemCommand extends MessageBase {
        constructor(public ItemId?: string, public ItemName?: string, public Quantity?: number, public Hide?: boolean) { super('DropItemCommand'); }
        public static ClassName: string = 'DropItemCommand';
    }
    export class ItemOwnershipMessage extends MessageBase {
        constructor(public Items?: ItemDescription[], public Giver?: ActorDescription, public Taker?: ActorDescription, public Hide?: boolean) { super('ItemOwnershipMessage'); }
        public static ClassName: string = 'ItemOwnershipMessage';
    }
    export class CashTransferMessage extends MessageBase {
        constructor(public Currency?: CurrencyDescription, public Quantity?: number, public Giver?: ActorDescription, public Taker?: ActorDescription, public Hide?: boolean) { super('CashTransferMessage'); }
        public static ClassName: string = 'CashTransferMessage';
    }
    export class LoginRejected extends MessageBase {
        constructor(public RejectionMessage?: string, public NoCharacter?: boolean) { super('LoginRejected'); }
        public static ClassName: string = 'LoginRejected';
    }
    export class LoginStateMessage extends MessageBase {
        constructor(public Actor?: ActorDescription, public Login?: boolean) { super('LoginStateMessage'); }
        public static ClassName: string = 'LoginStateMessage';
    }
    export class LookMessage extends MessageBase {
        constructor(public Direction?: Direction, public Actor?: number, public Item?: number, public YouFigureItOut?: string, public Brief?: boolean) { super('LookMessage'); }
        public static ClassName: string = 'LookMessage';
    }
    export class MoveMessage extends MessageBase {
        constructor(public Direction?: Direction, public Portal?: number) { super('MoveMessage'); }
        public static ClassName: string = 'MoveMessage';
    }
    export class PartyMovementMessage extends MessageBase {
        constructor(public Leader?: ActorDescription, public Actors?: ActorDescription[], public Direction?: Direction, public Enter?: boolean, public CustomText?: string) { super('PartyMovementMessage'); }
        public static ClassName: string = 'PartyMovementMessage';
    }
    export class PingMessage extends MessageBase {
        constructor(public SendTime?: Date) { super('PingMessage'); }
        public static ClassName: string = 'PingMessage';
    }
    export class PongMessage extends MessageBase {
        constructor(public SendTime?: Date) { super('PongMessage'); }
        public static ClassName: string = 'PongMessage';
    }
    export class RoomDescriptionMessage extends MessageBase {
        constructor(public RoomId?: number, public Name?: string, public Description?: string, public Actors?: ActorDescription[], public VisibleItems?: ItemDescription[], public FoundItems?: ItemDescription[], public VisibleCash?: CurrencyDescription[], public FoundCash?: CurrencyDescription[], public Exits?: ExitDescription[], public IsCurrentRoom?: boolean, public CannotSee?: boolean, public CannotSeeMessage?: string, public LightLevel?: LightLevel) { super('RoomDescriptionMessage'); }
        public static ClassName: string = 'RoomDescriptionMessage';
    }
    export class SearchCommand extends MessageBase {
        constructor(public Direction?: Direction) { super('SearchCommand'); }
        public static ClassName: string = 'SearchCommand';
    }
    export class SearchMessage extends MessageBase {
        constructor(public FoundItems?: ItemDescription[], public FoundCash?: CurrencyDescription[]) { super('SearchMessage'); }
        public static ClassName: string = 'SearchMessage';
    }
    export class ServerMessage extends MessageBase {
        constructor(public Contents?: string) { super('ServerMessage'); }
        public static ClassName: string = 'ServerMessage';
    }
    export class SysopMessage extends MessageBase {
        constructor(public Command?: string) { super('SysopMessage'); }
        public static ClassName: string = 'SysopMessage';
    }
    export class ItemDescription {
        public Name: string;
        public Id: string;
        public TemplateId: number;
        public Modified: boolean;
        public EquippedSlot: EquipmentSlot;
    }
    export class ActorDescription {
        public Name: string;
        public Id: string;
    }
    export class CurrencyDescription {
        public Name: string;
        public Amount: number;
    }
    export class ExitDescription {
        public Direction: Direction;
        public Name: string;
        public DestinationRoom: number;
    }
    export enum EquipmentSlot {
        Weapon = 0,
        Offhand = 1,
        Head = 2,
        Chest = 3,
        Legs = 4,
        Hands = 5,
        Feet = 6,
        Face = 7,
        Arms = 8,
        Shoulders = 9,
        Back = 10,
        Pocket = 11,
        Waist = 12,
        Eyes = 13,
        Ears = 14,
        Neck = 15,
        Wrists = 16,
        Fingers = 17,
        Light = 18,
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
