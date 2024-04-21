import { useEffect, useState } from 'react';
import searchIcon from '../../../assets/Search.svg';
import searchStyle from './styles/search.module.scss';
import reviewStyle from "./styles/review.module.scss";
import paginationStyle from './styles/pagination.module.scss';
import reviewTabStyle from './styles/reviewTab.module.scss';
import {ReviewAccordion} from "./components/ReviewAccordion.jsx";
import {ReviewsPagination} from "./components/ReviewsPagination.jsx";
import {ReviewSortSelect} from "./components/ReviewSortSelect.jsx";
import {userService} from "../../../services/user.service.js";

const PersonalReviewsTab = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const [sortType, setSortType] = useState("rating");
  const [pageCount, setPageCount] = useState(1);
  const [pageReviews, setPageReviews] = useState([]);
  const [search, setSearch] = useState('');

  async function handleSearchSubmit(event) {
      event.preventDefault();
      
      try {
        const count = await userService.getReviewsPagesCount(search);
        setPageCount(count);
        
        const revs = await userService.getReviews(sortType, search, currentPage);
        setPageReviews(revs);
        
      } catch(error) {
        console.log(error);
      }
  }
  
  useEffect(() => {
    (async () => {
      try {
        const count = await userService.getReviewsPagesCount(search);
        setPageCount(count);
        
        const revs = await userService.getReviews(sortType, search, currentPage);
        setPageReviews(revs);
        
      } catch (error) {
        console.log(error);
      }
    })()
  }, [currentPage])

  return (
        <div className={reviewTabStyle.reviewTab}>
            <div className={searchStyle.searchFilterBox}>
              <form className={searchStyle.searchForm} onSubmit={handleSearchSubmit} id={"search-form"}>
                <div className={searchStyle.searchInputWrapper}>
                  <input className={searchStyle.searchInput} placeholder={"Поиск по слову"} type={"text"}
                          name={"search"}
                          onChange={(event) => setSearch(event.target.value)}/>
                  <input type={"image"} src={searchIcon} width={45} height={45} alt={"Submit"}/>
                </div>
                <div className={searchStyle.sortSelectWrapper}>
                  <p className={searchStyle.sortLabel}>Сортировать по:</p>
                  <ReviewSortSelect setSortType={setSortType}/>
                </div>
              </form>
            </div>
            <div className={reviewStyle.reviewsBox}>
                {pageReviews.map(review => (
                  <ReviewAccordion key={review.id} review={review}></ReviewAccordion>
                ))}
            </div>
            <div className={paginationStyle.pagination}>
                <ReviewsPagination pageCount={pageCount} currentPage={currentPage} setCurrentPage={setCurrentPage}/>
            </div>
        </div>
    )
}

export default PersonalReviewsTab