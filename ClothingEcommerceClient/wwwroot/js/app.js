(function () {
    // Function to handle scrolling behavior
    window.onscroll = function () {
        var toplist = document.querySelector('.toplist');
        var searchbar = document.querySelector('.searchbar');
        var navbar = document.querySelector('.navbar');

        // Define scroll thresholds
        var toplistHideThreshold = 50;  // Hide TopList after 50px
        var searchbarHideThreshold = 350; // Hide SearchBar after 350px

        // Hide the toplist when scrolled more than toplistHideThreshold
        if (document.body.scrollTop > toplistHideThreshold || document.documentElement.scrollTop > toplistHideThreshold) {
            toplist.classList.add('toplist-hidden');
            navbar.style.top = '0'; // Move the navbar to the top
            searchbar.style.top = navbar.offsetHeight + 'px'; // Move the searchbar up below the navbar
        } else {
            toplist.classList.remove('toplist-hidden');
            navbar.style.top = '25px'; // Move navbar below TopList
            searchbar.style.top = (25 + navbar.offsetHeight) + 'px'; // Adjust SearchComponent position accordingly
        }

        // Hide the searchbar when scrolled more than searchbarHideThreshold
        if (document.body.scrollTop > searchbarHideThreshold || document.documentElement.scrollTop > searchbarHideThreshold) {
            searchbar.classList.add('searchbar-hidden');
        } else {
            searchbar.classList.remove('searchbar-hidden');
        }

        // Make the navbar fixed after scrolling past the toplist and searchbar
        var scrollPosition = document.body.scrollTop || document.documentElement.scrollTop;
        if (scrollPosition > toplist.offsetHeight + searchbar.offsetHeight) {
            navbar.classList.add('navbar-fixed'); // Apply fixed styling to the navbar
        } else {
            navbar.classList.remove('navbar-fixed'); // Revert to original state
        }
    };
})();


