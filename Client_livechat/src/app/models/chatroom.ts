import { Message } from "./message";
import { User } from "./user";

export class Chatroom {
    crId: string | undefined;
    titl: string | undefined;
    desc: string | undefined;
    usrs: User[] | undefined;
    mges: Message[] | undefined;

}
