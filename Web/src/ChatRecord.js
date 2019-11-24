//聊天记录类
class ChatRecord {
    constructor(sender,receiver,message, date, isSend) {
      this.sender = sender;
      this.receiver = receiver;
      this.message = message;
      this.date = date;
      this.isSend = isSend;
    }
  }

  export default ChatRecord;