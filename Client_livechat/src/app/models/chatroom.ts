import { Message } from "./message";
import { User } from "./user";

export class Chatroom {
    CRId: string | undefined;
    Titl: string | undefined;
    Desc: string | undefined;
    Usrs: User[] | undefined;
    Mges: Message[] | undefined;

}
