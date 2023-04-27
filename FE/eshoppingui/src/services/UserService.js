import axios from "axios";

export const LogIn = async (username, password, handleAlert) =>
{
    try{
        if(username === "" || password === "")
        {
            handleAlert("Enter both fields, username and password.", "error");
            return;
        }
        const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/login`,
        {
            username,
            password
        }); 

        const { token, ...logedInUser } = response.data; 
        localStorage.setItem("token", token);
        localStorage.setItem("logedInUser", JSON.stringify(logedInUser));
        handleAlert("Successfully loged in.", "success");
        
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to log in: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const RegisterUser = async (username, 
    email, 
    password,
    passwordRepeat, 
    name, 
    lastname, 
    dateOfBirth, 
    adress, 
    userType,
    profilePicture,
    handleAlert) =>
{
    try{   
        if(username === "" || email === "" || password === "" || passwordRepeat === "" || name === "" || lastname === "" || dateOfBirth === "" || adress === "" || userType === "")
        {
            handleAlert("All fields are mandatory.", "error");
            return;
        }
        if(password !== passwordRepeat)
        {
            handleAlert("Passwords does not match.", "error");
            return;
        } 

        const formData = new FormData();
        formData.append("username", username);
        formData.append("email", email);
        formData.append("password", password);
        formData.append("passwordRepeat", passwordRepeat);
        formData.append("name", name);
        formData.append("lastname", lastname);
        formData.append("dateOfBirth", dateOfBirth);
        formData.append("adress", adress);
        formData.append("userType", userType);
        formData.append("profilePicture", profilePicture);



        const response = await axios.post(
            `${process.env.REACT_APP_API_BASE_URL}/register`, 
            formData);

        handleAlert("Successfully registered, proceed to login page.", "success");
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to register: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}


export const UpdateProfile = async (
    username,
    name, 
    lastname,
    email,
    dateOfBirth,
    adress,
    profilePicture,
    handleAlert
) =>
{
    

    try{   
        if(name === "" || email === "" || lastname === "" || dateOfBirth === "" || adress === "")
        {
            handleAlert("You can't delete your info, you can only change it.", "error");
            return;
        }

        const formData = new FormData(); 
        formData.append("username", username);
        formData.append("email", email); 
        formData.append("name", name);
        formData.append("lastname", lastname);
        formData.append("dateOfBirth", dateOfBirth);
        formData.append("adress", adress); 
        formData.append("profilePicture", profilePicture);



        const response = await axios.put(
            `${process.env.REACT_APP_API_BASE_URL}/users/${username}/update`, 
            formData);

        handleAlert("Successfully registered, proceed to login page.", "success");
        return response;
    }
    catch(ex)
    {
        console.error(ex);
        console.error("Error while trying to update: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}