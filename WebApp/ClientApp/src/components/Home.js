import React, {Component} from 'react';
import {Contact} from "./Contact";
import {authorizedFetch} from "../Utils/authorizedFetch";
import * as signalR from "@microsoft/signalr";
import {Symbol} from "./Symbol/Symbol";


export class Home extends Component {
    static displayName = Home.name;
    state = {
        text: '',
        contactDtos: [],
        selectedPeerName: "",
    }
    myInfo = null;
    // refs
    chatInput = null;
    chatText = null;
    chatPlace = null;

    borderColorClasses = ["border-color-1", "border-color-2", "border-color-3", "border-color-4"];

    // todo сейчас буквы эпилептично раскрашиваются, потом надо чтоб они красились в зависимости от контакта
    // и для этого сначала надо сделать чтоб сообщения отправлялись кому надо а не всем подряд
    getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //Максимум не включается, минимум включается
    }

    async componentDidMount() {
        // signalr
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("default")
            .configureLogging(signalR.LogLevel.Debug)
            .build();
        await this.hubConnection.start();
        this.hubConnection.on("Send", data => this.textChange(data.symbol))
        this.hubConnection.on("DeleteSymbols", data => this.textDelete(data))
        this.hubConnection.on("GetStartedString", data => this.getStartedString(data))
        // contact list
        let contactGetResponse = await authorizedFetch('contact/this-user-contacts');
        if (contactGetResponse.ok) {
            this.setState({
                contactDtos: await contactGetResponse.json(),
            })
        } else {
            // у данного запроса нет 400-х ошибок, значит произошла жопа
            alert('идентити опять бесится, перелогинься')
        }

        // это все надо бы в один запрос или как то со страницей передать
        // my info
        let myInfoResponse = await authorizedFetch('user/my-info');
        if (!myInfoResponse.ok) {
            // у данного запроса нет 400-х ошибок, значит произошла жопа
            alert('идентити опять бесится, перелогинься')
        }
        this.myInfo = await myInfoResponse.json();

        setTimeout(() => this.hubConnection.invoke("GetStartedString"), 500);
        setInterval(() => this.hubConnection.invoke("CheckSymbols"), 1000);
    }

    textChange(newText) {
        this.setState({
            text: this.state.text + newText,
        });
        if (newText !== "")
            setTimeout(() => {
                if (this.chatText.scrollHeight > this.chatPlace.clientHeight - 10)
                    this.scrollDown();
            }, 10)
    }


    textDelete(count) {
        this.setState({
            text: this.state.text.slice(count),
        });
    }

    getStartedString(str) {
        this.setState({
            text: str
        });
    }

    selectPeer(screenName) {
        this.setState({
            selectedPeerName: screenName,
        });
    }

    scrollDown() {
        this.chatText.scrollTop = this.chatText.scrollHeight;
    }

    scrollHandle() {
        this.chatInput.blur()
        let scrollHeight = this.chatText.scrollHeight;
        let scrollTop = this.chatText.scrollTop
        // надо будет вообще гибкий макет делать такта))0
        if (scrollHeight - scrollTop === 800) // 800 высота чатека, надо будет взять из свойства
            setTimeout(() => {
                this.chatInput.focus()
            }, 11);
    }

    render() {
        return (
            <div className="main-page">
                <div className="user-contacts">
                    <ul className="list-group list-group-flush contacts">
                        <Contact avatarLink="https://i.kym-cdn.com/entries/icons/original/000/013/564/doge.jpg"
                                 screenName="sample_comtact_umgmg"/>
                        {this.state.contactDtos.map(value =>
                            <Contact avatarLink={value.avatarLink} screenName={value.screenName}
                                     onClick={() => this.selectPeer(value.screenName)}/>
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
                            {this.state.text.split('').map(value =>
                                <Symbol content={value}
                                        borderColorClass={this.borderColorClasses[this.getRandomInt(0, 3)]}/>
                            )}
                            <input type="text"
                                   className="chat-input"
                                   ref={(input) => {
                                       this.chatInput = input
                                   }}
                                   value=""
                                   onChange={(async event => {
                                       let symbol = event.target.value;
                                       this.textChange(symbol);
                                       // await this.hubConnection.invoke("Send", null, symbol)
                                       await this.hubConnection.invoke("Send", {
                                           author: this.myInfo.screenName,
                                           symbol: symbol,
                                           // shelfLife: Date.now().toString(),
                                           receiver: {
                                               isPrivate: true,
                                       //         todo защиту на бэке от посылания тем, кого нет в твоих контактах. вообще валидация на бэке важнее чем на фронте в тыщу раз
                                               screenName: this.state.selectedPeerName,
                                           },
                                       })
                                   })
                                   }
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
