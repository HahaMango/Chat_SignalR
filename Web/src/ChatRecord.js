//聊天记录类
let count = 0;

class ChatRecord {
    constructor(sender,receiver,message, date, isSend) {
      this.Sender = sender;
      this.Receiver = receiver;
      this.Message = message;
      this.Date = date;
      this.IsSend = isSend;
    }

    get Id(){
      return count++;
    }
  }

  export default ChatRecord;