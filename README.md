# 一个基于SignalR的在线聊天工具

## 功能介绍

- 临时用户名登陆，不需注册，输入临时用户名登陆系统。
- 全局广播信息，在线的所有用户都能收到的全局广播信息。
- 点对点的通信，在右边用户列表中选择一个用户即可与该用户进行通信。
- 用户列表，分页获取一定数量的在线用户然后更新右边的用户列表。

## UI

前端UI是用的`Vue.js`，配合`signalr.js`来做的跟服务器的实时通信。

![ui界面](https://github.com/HahaMango/Chat_SignalR/blob/master/img/Demo.jpg)

vue项目在web文件夹中。

### vue构建

`cd`到`web`文件夹中运行

`npm run build`

### 运行webpack-server

`npm start`

## 服务端

服务端使用`VS 2017`，`asp.net core 2.2`构建，使用`SignalR version 1.1.4`作为实时通信库
