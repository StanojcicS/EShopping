import React, {useState} from "react"
import { Link } from "react-router-dom";
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css';
import { RegisterUser } from "../services/UserService"; 

export const Register = () => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordRepeat, setPasswordRepeat] = useState('');
    const [name, setName] = useState('');
    const [lastname, setLastname] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState('');
    const [adress, setAdress] = useState('');
    const [userType, setUserType] = useState('seller');
    const [profilePicture, setProfilePicture] = useState(null);
 

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        await RegisterUser(username, email, password, passwordRepeat, name, lastname, dateOfBirth, adress, userType, profilePicture, handleAlert);
        
    }

    return(
        <div className="auth-form-container">
            <ToastContainer/>
            <h2>Register</h2>
            <form className="register-form" onSubmit={handleSubmit}>
                <label htmlFor="username">username</label>
                <input value={username} onChange={(e) => setUsername(e.target.value)} type="username" placeholder="username" />
                <label htmlFor="email">email</label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" name="email"/> 
                <label htmlFor="password">password</label>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="password" id="password" name="password"/> 
                <label htmlFor="repeatPassword">repeat password</label>
                <input value={passwordRepeat} onChange={(e) => setPasswordRepeat(e.target.value)} type="password" placeholder="password" id="passwordRepeat" name="passwordRepeat"/> 
                <label htmlFor="name">name</label>
                <input value={name} onChange={(e) => setName(e.target.value)} type="name" placeholder="name" />
                <label htmlFor="lastName">lastname</label>
                <input value={lastname} onChange={(e) => setLastname(e.target.value)} type="lastname" placeholder="lastname" />
                <label htmlFor="dateOfBirth">date of birth</label>
                <input type="datetime-local" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} id="dateOfBirth" name="dateOfBirth"></input>
                <label htmlFor="adress">adress</label>
                <input value={adress} onChange={(e) => setAdress(e.target.value)} type="adress" placeholder="adress" />
                <label htmlFor="userType">user type</label>
                <select className="userTypeDropdown" value={userType} onChange={(e) => setUserType(e.target.value)} id="userType" name="userType">
                    <option value="seller">Seller</option>
                    <option value="buyer">Buyer</option>
                </select>
                <label htmlFor="profilePicture">Profile picture:</label>
                <input type="file" accept="image/*" onChange={(e) => setProfilePicture(e.target.files[0])} id="profilePicture" name="profilePicture" />
                
                <button type="submit">Register</button>
            </form> 
            <button className="link-button">
                <Link className="link-text" to="/login">
                    Already have an account? Log in here.
                </Link>
            </button>
        </div>
    )
}