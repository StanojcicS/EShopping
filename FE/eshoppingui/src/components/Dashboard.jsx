import { toast, ToastContainer } from "react-toastify"
import { AddNewArticleButton } from "./dashboard-buttons/AddNewArticleButton"
import { AllOrdersButton } from "./dashboard-buttons/AllOrdersButton"
import { MyOrdersButton } from "./dashboard-buttons/MyOrdersButton"
import { NewOrderButton } from "./dashboard-buttons/NewOrderButton"
import { NewOrdersButton } from "./dashboard-buttons/NewOrdersButton"
import { OldOrdersButton } from "./dashboard-buttons/OldOrdersButton"
import { VerifyUsersButton } from "./dashboard-buttons/VerifyUsersButton"
import { MyProfileButton } from "./dashboard-buttons/MyProfileButton"

const buttonsToRender = {
    Admin: [<MyProfileButton/>, <AllOrdersButton/>, <VerifyUsersButton/>],
    Seller: [<MyProfileButton/>, <AddNewArticleButton/>, <MyOrdersButton/>, <NewOrdersButton/>],
    Buyer: [<MyProfileButton/>, <NewOrderButton/>, <OldOrdersButton/>]
}

export const Dashboard = () => {
    const logedInUser = JSON.parse(localStorage.getItem("logedInUser"));
    const buttons = buttonsToRender[logedInUser.userType];

    const tableRows = []; 
    for(let i = 0; i < buttons.length; i+=2)
    { 
        if(i + 1< buttons.length)
        {
            const newRow = (
                <tr key={i}>
                    <td>{buttons[i]}</td> 
                    <td>{buttons[i+1]}</td>
                </tr>
            )
            tableRows.push(newRow);
        }
        else
        {
            const newRow = (
                <tr>
                    <td>{buttons[i]}</td>  
                </tr>
            ) 
            tableRows.push(newRow);
        }
    }

    return(
        <div className="dashboard-container">
            
        <ToastContainer/>
            <table>
                <tbody>
                    {tableRows}
                </tbody>
            </table>
        </div>
        )
}