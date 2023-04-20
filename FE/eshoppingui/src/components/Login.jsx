import React, {useState} from "react"
import { Link } from "react-router-dom";
import { LogIn } from "../services/UserService.js";
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css';

export const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const handleLogin = async (e) => { 
        e.preventDefault(); 
        await LogIn(username, password, handleAlert);            
    }

    return(
        <div className="auth-form-container">
            <h2>Log in</h2>
            <form className="login-form" onSubmit={handleLogin}>
                <label htmlFor="username">username</label>
                <input value={username} onChange={(e) => setUsername(e.target.value)} type="username" placeholder="yourusername" id="username" name="username"/> 
                <label htmlFor="password">password</label>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="password" id="password" name="password"/> 
                <button type="submit">Log in</button>
            </form>
            <button className="link-button">
                <Link className="link-text" to='/register'>
                    Don't have an account? Register here.
                </Link>
            </button>
            <ToastContainer />
        </div>
    )
}