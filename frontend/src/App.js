import Header from "./Components/Header";
import HomePage from "./Pages/HomePage";
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import NotFoundPage from "./Pages/NotFoundPage";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

const router = createBrowserRouter([
    {
        path: "/",
        element: <Header><HomePage/></Header>,
        errorElement: <NotFoundPage />,
    }
]);
function App() {
  return (
      <>
          <RouterProvider router={router} />
          <ToastContainer />
      </>
  );
}

export default App;
