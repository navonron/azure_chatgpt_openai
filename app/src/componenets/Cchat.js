import React, { useState, useEffect, useRef } from "react";
import "../css/chat.css";
import axios from "axios";
import isracard_iogo from "../assets/isracard_iogo.png";
import send from "../assets/send.png";
import text from "../elements/Etext";
import Cmassage from "./Cmassage";
import CircularProgress from "@mui/material/CircularProgress";

const Cchat = ({ useremail }) => {
  React.useEffect(() => {
    sessionStorage.clear();
    localStorage.clear();
  }, []);

  const [inputMessage, setInputMessage] = useState("");
  const [load, setLoad] = useState(true);
  const [messages, setMessages] = useState([]);

  const handleInputChange = (e) => {
    setInputMessage(e.target.value);
  };

  const handleSendMessage = async () => {
    if (inputMessage.trim() === "") {
      return;
    }

    // Add the user message to the state
    const userMessage = { text: inputMessage, isUser: true };
    setLoad(false);
    setMessages((prevMessages) => [...prevMessages, userMessage]);
    setInputMessage("");

    try {
      const clientIP = await axios
        .get(text.getIP)
        .then((response) => response.data.ip);

      const response = await axios.post(
        text.apiURL +
          "/api/csv/NewQuestion?NewQuestion=" +
          inputMessage +
          "&clientIP=" +
          clientIP,
        {},
        {
          headers: {
            userEmail: useremail,
          },
        }
      );

      // Assuming your API returns the response message in the "message" field
      const apiResponseMessage = response.data;

      // Add the API response to the state
      const apiMessage = { text: apiResponseMessage, isUser: false };
      setLoad(true);
      setMessages((prevMessages) => [...prevMessages, apiMessage]);
    } catch (error) {
      console.error("Error sending message:", error);
    }
  };

  return (
    <div>
      <Cmassage />
      <div class="chat">
        <div class="chat-title">
          <h1>{text.chatHeader}</h1>
          <figure class="avatar">
            <img src={isracard_iogo} />
          </figure>
        </div>
        <div class="messages">
          <div class="messages-content" style={{ overflow: "auto" }}>
            {messages.map((message, index) => (
              <div
                key={index}
                className={`message ${message.isUser ? "user" : "api"}`}
              >
                {message.text}
              </div>
            ))}
          </div>
        </div>
        <div>
          <CircularProgress disableShrink hidden={load} />
        </div>
        <div class="message-box">
          <textarea
            type="text"
            class="message-input"
            placeholder="Type message..."
            value={inputMessage}
            onChange={handleInputChange}
          ></textarea>
          <button
            type="submit"
            class="message-submit"
            onClick={handleSendMessage}
          >
            <img src={send} className="send_img" alt="Send" />
          </button>
        </div>
      </div>
      <div class="bg"></div>
    </div>
  );
};

export default Cchat;
