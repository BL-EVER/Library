import React, {useEffect, useState} from 'react';
import BookList from "../Components/BookList";
import CategoryModal from "../Components/CategoryModal";
import BookModal from "../Components/BookModal";
import ProtectedComponentWrapper from "../Utilities/ProtectedComponentWrapper";

const HomePage = () => {
    const [openCategory, setOpenCategory] = useState(false);
    const [category, setCategory] = useState({});
    const handleAddCategory = (category) => {
        setCategory(category);
        setOpenCategory(true);
    }
    const [openBook, setOpenBook] = useState(false);
    const [book, setBook] = useState({});
    const handleAddBook = (book) => {
        setBook(book);
        setOpenBook(true);
    }

    const [refresh, setRefresh] = useState(1);
    useEffect(() =>
        {
            if (!openCategory || !openBook) setRefresh(Math.floor(Math.random() * 1000));
        },
        [openCategory, openBook]
    );
    return (
        <div style={{margin: "8px"}}>
            <BookList handleBookModal={handleAddBook} handleCategoryModal={handleAddCategory} refresh={refresh} />
            <ProtectedComponentWrapper authorizationRoles={["admin"]}>
                <CategoryModal category={category} open={openCategory} setOpen={setOpenCategory} />
                <BookModal book={book} open={openBook} setOpen={setOpenBook} />
            </ProtectedComponentWrapper>
        </div>
    );
};

export default HomePage;