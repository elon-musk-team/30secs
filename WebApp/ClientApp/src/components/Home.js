import React, { Component } from 'react';


export class Home extends Component {
	static displayName = Home.name;
	state = {
		text: '',
		firstLine: '',
		firstLineLength: 0
	}
	textChange(text){
		// if (this.state.firstLineLength > 125){
		// 	console.log("больше");
		// 	this.setState({
		// 		text: this.state.text + this.state.firstLine,
		// 		firstLine: text,
		// 		firstLineLength: this.state.firstLine.length
		// 	})
		// } else {
		this.setState({
			text: this.state.text + text,
		});
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
						<li>Иван Домашних</li>
					</ul>
				</div>
				<div className="chat-place" onClick={() => {
					document.querySelector(".chat-input").focus();
				}}>

					<div className="chat-text-wrap">
						<div className="chat-container">
							<div className="chat-text">{this.state.text}
								<input type="text"
								       className="chat-input"
								       value=""
								       onChange={(event => {this.textChange(event.target.value)})}
								/></div>

						</div>
					</div>
				</div>
			</div>
		);
	}
}
