import { useState } from 'react'
import './PersonalReviewsTab.scss'
import {createTheme, Pagination, ThemeProvider} from "@mui/material";
const PersonalReviewsTab = () => {
    // TODO: ajax запрос
    const reviews = [
        [
          {
            id: 1,
            isPositive: true,
            name: "Круто, я не смотрел",
            rating: 10,
            contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
            updatedAt: "22.8.2024",
            text: "Это тут насрали на кровать?"
          },
          {
            id: 2,
            isPositive: false,
            name: "Круто?",
            rating: 2,
            contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
            updatedAt: "22.8.2024",
            text: "aga aga aga aga"
          }
        ],
        [
          {
            id: 3,
            isPositive: true,
            name: "Занимайся жизнью или занимайся смертью",
            rating: 10,
            contentName: "Побег из Шоушенка",
            updatedAt: "1.1.2001",
            text: "xdd"
          }
        ]
    ];

    const [currentPage, setCurrentPage] = useState(1);
    
    return (
        <div className={"reviews-tab-wrapper"}>
            <div className={"search-filter-box"}>
                <form id={"search-form"}>
                    <input placeholder={"Поиск по слову"} type={"text"} name={"search"}/>
                    <input type={"image"} alt={"Submit"}/>
                </form>
                <button id={"filter-btn"}>Фильтры</button>
            </div>
            <div className={"reviews-box"}>
                {reviews[currentPage - 1].map(review => (
                  <ReviewAccordion key={review.id} review={review}></ReviewAccordion>
                ))}
            </div>
            <div className={"pagination"}>
                <ReviewsPagination pageCount={reviews.length} currentPage={currentPage} setCurrentPage={setCurrentPage}/>
            </div>
        </div>
    )
}

export default PersonalReviewsTab

const ReviewAccordion = ({review}) => {
    const [isActive, setIsActive] = useState(false);
    
    return (
        <div className={"accordion-item " + (isActive ? "review-block-opened" : "review-block")} data-id={review.id}>
            <div className={"accordion-title"} onClick={() => setIsActive(!isActive)}>
                <div className={"rating " + (review.isPositive ? "positive" : "negative")}>
                    <p>{review.rating}</p>
                </div>
                <div className={"review-info"}>
                    <p className={"content-name"}>{review.contentName}</p>
                    <p className={"review-name"}>{review.name}</p>
                    <p className={"date"}>{review.updatedAt}</p>
                </div>
                {!isActive && <p className={"review-text-beginning"}>{review.text.slice(0, 150) + (review.text.length > 150 ? "..." : "")}</p>}
            </div>
            {isActive && <div className={"accordion-content"}>
                <p className={"review-text"}>{review.text}</p>
            </div>}
        </div>
    );
}

const ReviewsPagination = ({pageCount, currentPage, setCurrentPage}) => {
    const theme = createTheme({
        palette: {
            netflix: {
                main: '#E50914',
                light: '#F51924',
                dark: '#C50004',
                contrastText: '#FFFFFF',
            },
        },
        components: {
            MuiPagination: {
                styleOverrides: {
                    root: {
                        button: {
                            color: "#fff"
                        }
                    }
                }
            }
        }
    });

    const paginationStyle = {
        position: "absolute",
        left: 0,
        right: 0,
        bottom: 0,
        marginLeft: "auto",
        marginRight: "auto",
        width: "fit-content",
        padding: "1em",
        backgroundColor: "black",
        borderRadius: "1.2em"
    };
    
    function handlePageChange(event, page) {
        setCurrentPage(page);
    }

    return (
      <ThemeProvider theme={theme}>
          <Pagination count={pageCount} color={"netflix"} size={"large"} 
                      page={currentPage} onChange={handlePageChange} 
                      style={paginationStyle}/>
      </ThemeProvider>
    );
}