TODO: exclude/include some calendars (bergs-only, us-only...)

# Date formatting for holidays

## Static date
Always use two digits for the month and day of the month.

| Date | Format |
|------|--------|
| May 2 | <span style="white-space:nowrap">05-02</span> |
| October 19 | <span style="white-space:nowrap">10-19</span> |
| December 31 | <span style="white-space:nowrap">12-31</span> |

## Floating date
Use a single digit for n (except for _last_, which uses “L”) and the day of the week. Days of the week are numbered starting with one for Sunday.

| Date | Format |
|------|--------|
| First Wednesday in October | <span style="white-space:nowrap">10-N1-4</span> |
| Fourth Sunday in June | <span style="white-space:nowrap">06-N4-1</span> |
| Last Monday in May | <span style="white-space:nowrap">05-NL-2</span> |

## Relative dates

When the holiday is _relative to_ another date, append an underscore and the number of days to add to get the correct date.

| Date | Format |
|------|--------|
| Christmas Eve | <span style="white-space:nowrap">12-25_-1</span> _or_ <span style="white-space:nowrap">12-24</span> |
| National Napping Day (day after daylight time begins) | <span style="white-space:nowrap">03-N2-1_1</span> _or_ <span style="white-space:nowrap">03-N2-1_+1</span> |
| National Teacher Appreciation Day (Tuesday of Teacher Appreciation Week, the first full week of May) | <span style="white-space:nowrap">05-N1-1_2</span> _or_ <span style="white-space:nowrap">05-N1-1_+2</span> |
| Black Friday (day after the fourth Thursday in November, which is _not necessarily_ the fourth Friday; see 2024) | <span style="white-space:nowrap">11-N4-5_1</span> _or_ <span style="white-space:nowrap">11-N4-5_+1</span> |

## Incalculable
Some dates are difficult or impossible to calculate. These dates will require manual intervention to calculate.

For the date format, use an exclamation point followed by any note you’d like displayed when the calendar is generated.

| Holiday | Format |
|------|--------|
| Vernal equinox | <span style="white-space:nowrap">!Around March 20</span> |
| Palm Sunday | <span style="white-space:nowrap">!Sunday before Easter</span> |
| Easter | <span style="white-space:nowrap">!Google it lol</span> |