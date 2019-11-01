//聊天记录类
class ChatRecord {
    constructor(communicator,message, date, isSend) {
      this.communicator = communicator;
      this.message = message;
      this.date = date;
      this.isSend = isSend;
    }
  }

  export default ChatRecord;