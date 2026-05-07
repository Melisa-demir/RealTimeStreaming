import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import "./App.css";

function App() {
    const [messages, setMessages] = useState([]);
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5027/streamingHub")
            .withAutomaticReconnect()
            .build();

        newConnection.on("ReceiveMessage", (user, message) => {
            setMessages((prev) => [...prev, `${user}: ${message}`]);
        });

        newConnection
            .start()
            .then(() => {
                console.log("SignalR connected");
                setConnection(newConnection);
            })
            .catch((err) => console.error("SignalR Connection Error: ", err));

        return () => {
            newConnection.stop();
        };
    }, []);

    const sendMessage = async () => {
        if (connection) {
            await connection.invoke("SendMessage", "Melisa", "Merhaba, gerçek zamanlı mesaj!");
        }
    };

    return (
        <div className="App">
            <h1>Real-Time Messages</h1>

            <button onClick={sendMessage}>Mesaj Gönder</button>

            <ul>
                {messages.map((msg, index) => (
                    <li key={index}>{msg}</li>
                ))}
            </ul>
        </div>
    );
}

export default App;