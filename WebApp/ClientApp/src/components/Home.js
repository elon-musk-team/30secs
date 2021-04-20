import React, { Component } from 'react';


export class Home extends Component {
	static displayName = Home.name;
	state = {
		text: '',
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
	scrollDown(){
		let chatBody = document.querySelector('.chat-text');
		chatBody.scrollTop = chatBody.scrollHeight;
	}
	scrollHandle(){
		document.querySelector(".chat-input").blur()
		let scrollHeight = document.querySelector('.chat-text').scrollHeight;
		let scrollTop = document.querySelector('.chat-text').scrollTop
		if (scrollHeight - scrollTop === 800) // 800 высота чатека, надо будет взять из свойства
			setTimeout(function () { document.querySelector(".chat-input").focus()}, 11);
	}
	// phraseComponent -
	// userContact - component сделать
	render () {
		return (
			<div className="main-page">
				<div className="user-contacts">
					<ul className="contacts">
						<li>Миша Пивасов</li>
						<li>Макар</li>
						<li>Димас</li>
						<li>Личка с собой???</li>
						<li>Надо нажать на область, чтоб писать -&gt;</li>
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
							       onChange={(event => {this.textChange(event.target.value)})}
							/>
						</div>
					</div>
				</div>
			</div>
		);
	}
}
