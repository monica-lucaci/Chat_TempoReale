import { Chatroom } from "./chatroom";

export class User {
    user: string | undefined;

    pass: string | undefined;
  
    img: string | undefined;
  
    myChatRooms: Chatroom[] | undefined;
}
