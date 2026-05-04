import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import logo from './logo.svg';
import './App.css';

function App() {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5002/streamingHub") // StreamingService URL'si
            .build();

        connection.on("ReceiveMessage", (user, message) => {
            setMessages((prev) => [...prev, `${user}: ${message}`]);
        });

        connection.start().catch(err => console.error("SignalR Connection Error: ", err));
    }, []);
                
        return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <p>
                    Edit <code>src/App.js</code> and save to reload.
                </p>
                <a
                    className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Learn React
                </a>
            </header>
            <div>
                <h1>Real-Time Messages</h1>
                <ul>
                    {messages.map((msg, index) => (
                        <li key={index}>{msg}</li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default App;
