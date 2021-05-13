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
        // защита от плохих данных происходит на бэке, поэтому могу себе позволить вставлять что угодно
        const data = await authorizedFetch(`contact/this-user-contacts?screenName=${inputData}`, {method: 'POST'})
        const result = document.getElementById('result');
        if (data.ok){
            result.innerText = `ok, ${inputData}`
        } else {
            result.innerText = `you have error ${data.status} and json ${JSON.stringify(await data.json())}`
        }
    }
    
    render() {
        return (
            <div>
                <p>надо написать сюда ScreenName чела которого хочешь добавить</p>
                <input id="screenname"/>
                <button className="btn btn-primary" onClick={this.incrementCounter}>Нажми f12 -{">"} сеть потом сюда</button>
                <p id="result">Здесь будет результат</p>
            </div>
        );
    }
}
