import { Chatroom } from "./chatroom";

export class User {

    email: string | undefined;
    
    user: string | undefined;

    pass: string | undefined;
  
    img: string | undefined;
  
    myChatRooms: Chatroom[] | undefined;
}
