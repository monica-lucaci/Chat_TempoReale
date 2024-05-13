import { Message } from "./message";
import { User } from "./user";

export class Chatroom {
    crId: string | undefined;
    crCd: string | undefined;
    titl: string | undefined;
    desc: string | undefined;
    crImg: string | undefined;
    usrs: User[] | undefined;
    mges: Message[] | undefined;

}
