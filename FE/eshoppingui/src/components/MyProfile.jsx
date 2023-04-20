export const MyProfile = () => {
    const logedInUser = JSON.parse(localStorage.getItem("logedInUser"));

    return(
        <h1>MyProfile</h1>
    );
}