import React, { Component } from 'react';
import {Contact} from "./Contact";
import {authorizedFetch} from "../Utils/authorizedFetch";
import * as signalR from "@microsoft/signalr";


export class Home extends Component {
	static displayName = Home.name;
	state = {
		text: '',
		contactDtos: [],
	}

	async componentDidMount() {
		// signalr
		this.hubConnection = new signalR.HubConnectionBuilder()
			.withUrl("default")
			.build();
		this.hubConnection.start();
		this.hubConnection.on("Send", data => this.textChange(data))
		this.hubConnection.on("DeleteSymbols", data => this.textDelete(data))
		this.hubConnection.on("GetStartedString", data => this.getStartedString(data))
		// contact list
		let contactGetResponse = await authorizedFetch('Contact/ThisUserContacts');
		if (contactGetResponse.ok) {
			this.setState({
				text: '',
				contactDtos: await contactGetResponse.json(),
			})
		} else {
			// у данного запроса нет 400-х ошибок, значит произошла жопа
			alert(await contactGetResponse.text())
		}
		setTimeout(() => this.hubConnection.invoke("GetStartedString"), 500);
		setInterval(() => this.hubConnection.invoke("CheckSymbols"), 1000);
	}

	textChange(text){
		this.setState({
			text: this.state.text + text,
		});
		if(text !== "")
			setTimeout(() => {
				if (document.querySelector('.chat-text').scrollHeight > document.querySelector('.chat-place').clientHeight - 10)
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
	
	scrollDown(){
		let chatBody = document.querySelector('.chat-text');
		chatBody.scrollTop = chatBody.scrollHeight;
	}
	
	scrollHandle(){
		document.querySelector(".chat-input").blur()
		let scrollHeight = document.querySelector('.chat-text').scrollHeight;
		let scrollTop = document.querySelector('.chat-text').scrollTop
		// надо будет вообще гибкий макет делать такта))0
		if (scrollHeight - scrollTop === 800) // 800 высота чатека, надо будет взять из свойства
			setTimeout(function () { document.querySelector(".chat-input").focus()}, 11);
	}
	// phraseComponent - сделать
	render () {
		return (
			<div className="main-page">
				<div className="user-contacts">
					<ul className="list-group list-group-flush contacts">
						<Contact avatarLink="https://i.kym-cdn.com/entries/icons/original/000/013/564/doge.jpg" screenName="sample_comtact_umgmg"/>
						{this.state.contactDtos.map(value => 
							<Contact avatarLink={value.avatarLink} screenName={value.screenName}/>
							)}
					</ul>
				</div>
				<div className="chat-place" onClick={() => {
					document.querySelector(".chat-input").focus();
					this.scrollDown()
				}}>
					<div className="chat-text-wrap">
						<div className="chat-text" onScroll={() => {this.scrollHandle()}}>{this.state.text}
							<input type="text"
							       className="chat-input"
								value=""
								onChange={(event => {
									this.textChange(event.target.value);
									this.hubConnection.invoke("Send", null, event.target.value)
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
