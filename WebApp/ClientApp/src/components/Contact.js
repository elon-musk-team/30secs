import {Component} from "react";

export class Contact extends Component{
    static displayName = Contact.name;
    
    render() {
        return (
            <li className="container contact-container row">
                <div className="col">
                    <div className="avatar-circle">
                        <img src={this.props.avatarLink} alt="-_-"/>
                    </div>
                </div>
                <div className="col">
                    <p className="text-truncate contact-label">{this.props.screenName}</p>
                </div>
            </li>
        )
    }
}
