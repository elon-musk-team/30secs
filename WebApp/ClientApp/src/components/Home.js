import React, {Component} from 'react';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";
import shortid from "shortid";
import {Symbol} from "./Symbol/Symbol";
import authService from "./api-authorization/AuthorizeService";
import {Contact} from "./Contact";
import {authorizedFetch} from "../Utils/authorizedFetch";


export class Home extends Component {
    static displayName = Home.name;
    state = {
        myInfo: {myContacts: [], screenName: '', linkToAvatar: ''},
        selectedPeerName: "",
        symbolDtos: [],
    }
    
    // refs
    chatInput = null;
    chatText = null;
    chatPlace = null;

    borderColorClasses = ["border-color-1", "border-color-2", "border-color-3", "border-color-4"];
    
    getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
    }

    async componentDidMount() {
        // signalr
        let accessToken = await authService.getAccessToken();
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("chatHub", { accessTokenFactory: () => accessToken , transport: HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents})
            .configureLogging(signalR.LogLevel.Trace)
            .build();
        await this.hubConnection.start();
        this.hubConnection.on("Send", data => this.handleReceive(data))
        this.hubConnection.on("DeleteSymbols", data => this.textDelete(data))
        this.hubConnection.on("GetStartedString", data => this.getStartedString(data))
        
        // my info
        let myInfoResponse = await authorizedFetch('user/my-info');
        if (!myInfoResponse.ok) {
            alert('пацаны продлите сессию у идентити')
        }
        let myInfo = await myInfoResponse.json();
        this.setState({
            myInfo: myInfo,
        })
        
        setInterval(() => this.hubConnection.invoke("CheckSymbols", {
            isPrivate: true,
            screenName: this.state.selectedPeerName,
        }), 5000);
    }

    handleReceive(newData) {
        this.textChange(newData);
    }
    
    textChange(newData) {
        if (!newData) {
            return;
        }
        this.setState(prevState => ({
            symbolDtos: [...prevState.symbolDtos, newData]
        }));
        setTimeout(() => {
            if (this.chatText.scrollHeight > this.chatPlace.clientHeight - 10)
                this.scrollDown();
        }, 10)
    }


    textDelete(count) {
        this.setState(prevState => ({
            symbolDtos: prevState.symbolDtos.slice(count),
        }));
    }

    getStartedString(startSymbols) {
        this.setState(() => ({
            symbolDtos: startSymbols,
        }));
    }

    async selectPeer(screenName) {
        await this.hubConnection.invoke("GetStartedString", {
            isPrivate: true,
            screenName: this.state.selectedPeerName,
        })
        this.setState({
            selectedPeerName: screenName,
        });
    }

    scrollDown() {
        this.chatText.scrollTop = this.chatText.scrollHeight;
    }

    scrollHandle(){
        this.chatInput.blur()
        let scrollHeight = this.chatText.scrollHeight;
        let scrollTop = this.chatText.scrollTop
        let height = this.mainPage.clientHeight;
        if (height - scrollHeight - scrollTop < 10) 
            setTimeout(() => {
                this.chatInput.focus()
            }, 11);
      }
    
    getBorderColorClass(symbolObject) {
            if (this.state.myInfo.screenName === symbolObject.receiver.screenName){
                return this.borderColorClasses[0];
            }
            return this.borderColorClasses[1];

    }
    
    async handleInput(event) {
        let symbolChar = event.target.value;
        let symbolObject = {
            symbol: symbolChar,
            // не буду передавать время, т.к. на сервере проставляется текущее
            // shelfLife: Date.now().toString(),
            receiver: {
                isPrivate: true,
                screenName: this.state.selectedPeerName,
            },
        };
        this.textChange(symbolObject);
        await this.hubConnection.invoke("Send", symbolObject);
    }
    
    render() {
        return (
            <div className="main-page" ref={(mainPage) => this.mainPage = mainPage}>
                <div className="user-contacts">
                    <ul className="list-group list-group-flush contacts">
                        {this.state.myInfo.myContacts.map(value =>
                            <Contact linkToAvatar={value.linkToAvatar} 
                                     screenName={value.screenName}
                                     key={value.screenName}
                                     isSelected={this.state.selectedPeerName === value.screenName}
                                     onClick={async () => {await this.selectPeer(value.screenName)}}/>
                        )}
                    </ul>
                </div>
                <div className="chat-place" ref={(chatPlace) => this.chatPlace = chatPlace} onClick={() => {
                    this.chatInput.focus();
                    this.scrollDown()
                }}>
                    <div className="messages-place"></div>
                    <div className="chat-text-wrap">
                        <div className="chat-text" id="chat-content" ref={(chatText) => {
                            this.chatText = chatText
                        }} onScroll={() => {
                            this.scrollHandle()
                        }}>
                            {this.state.symbolDtos.map((value, index) =>
                                <Symbol content={value.symbol}
                                        screenName={this.state.selectedPeerName === value.receiver.screenName ? this.state.myInfo.screenName : this.state.selectedPeerName}
                                        key={shortid()}
                                        isFirst={index === 0}
                                        isLast={index === this.state.symbolDtos.length - 1}
                                        borderColorClass={this.getBorderColorClass(value)}/>
                            )}
                            <input type="text"
                                   className="chat-input"
                                   ref={(input) => {
                                       this.chatInput = input
                                   }}
                                   value=""
                                   onChange={(async event => await this.handleInput(event))
                                   }
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
