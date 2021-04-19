import React, {Component} from "react";
import {authorizedFetch} from "../Utils/authorizedFetch";

export class AddContact extends Component{
    static displayName = AddContact.name;

    constructor(props) {
        super(props);
        this.incrementCounter = this.incrementCounter.bind(this);
    }

    async incrementCounter() {
        const inputData = document.getElementById('screenname').value
        // здесь очевидная дыра в безопасности но я чет тороплюсь
        const data = await authorizedFetch(`Contact/ThisUserContacts?screenName=${inputData}`, {method: 'POST'})
    }
    
    render() {
        return (
            <div>
                <p>надо написать сюда ScreenName чела которого хочешь добавить</p>

                <input id="screenname"/>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Нажми f12 -{">"} сеть потом сюда</button>
            </div>
        );
    }
}
