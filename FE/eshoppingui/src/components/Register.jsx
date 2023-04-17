import React, {useState} from "react"

export const Register = (props) => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [pass, setPass] = useState('');
    const [passRepeat, setPassRepeat] = useState('');
    const [name, setName] = useState('');
    const [lastname, setLastname] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState('');
    const [adress, setAdress] = useState('');
    const [userType, setUserType] = useState('seller');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.info(userType);
    }

    return(
        <div className="auth-form-container">
            <h2>Register</h2>
            <form className="register-form" onSubmit={handleSubmit}>
                <label htmlFor="username">username</label>
                <input value={username} onChange={(e) => setUsername(e.target.value)} type="username" placeholder="username" />
                <label htmlFor="email">email</label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@gmail.com" id="email" name="email"/> 
                <label htmlFor="password">password</label>
                <input value={pass} onChange={(e) => setPass(e.target.value)} type="password" placeholder="password" id="password" name="password"/> 
                <label htmlFor="repeatPassword">repeat password</label>
                <input value={passRepeat} onChange={(e) => setPassRepeat(e.target.value)} type="password" placeholder="password" id="passwordRepeat" name="passwordRepeat"/> 
                <label htmlFor="name">name</label>
                <input value={name} onChange={(e) => setName(e.target.value)} type="name" placeholder="name" />
                <label htmlFor="lastName">lastname</label>
                <input value={lastname} onChange={(e) => setLastname(e.target.value)} type="lastname" placeholder="lastname" />
                <label htmlFor="dateOfBirth">date of birth</label>
                <input type="datetime-local" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} id="dateOfBirth" name="dateOfBirth"></input>
                <label htmlFor="adress">adress</label>
                <input value={adress} onChange={(e) => setAdress(e.target.value)} type="adress" placeholder="adress" />
                <label htmlFor="userType">user type</label>
                <select  value={userType} onChange={(e) => setUserType(e.target.value)} id="userType" name="userType">
                    <option value="seller">Seller</option>
                    <option value="buyer">Buyer</option>
                </select>
                
                
                <button type="submit">Log in</button>
            </form>
            <button className="link-button" onClick={() => props.onFormSwitch('login')}>Already have an account? Log in here.</button>
        
        </div>
    )
}