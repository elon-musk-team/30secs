import {Component} from "react";

export class Contact extends Component{
    static displayName = Contact.name;
    
    render() {
        return (
            <li className="list-group-item flex-row align-items-start">
                <div className="row">
                    <div className="col">
                        <div className="avatar-circle">
                            <img src={this.props.avatarLink} alt="-_-"/>
                        </div>
                    </div>
                    <div className="col">
                        <p className="text-truncate contact-label flex-grow-1">{this.props.screenName}</p>
                    </div>
                </div>
                
            </li>
        )
    }
}
