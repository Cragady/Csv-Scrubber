# Purpose

The purpose of this little exercise is to pull data from a Microsoft Access table. In the end, I'll read the files back in for data analyzing.

This is pretty much a barebones console application. It's likely to be single use, so it likely will not have any upkeep in the future.

As of this commit (on 9/12/2020), this application is finished for my purposes. If I continue to work on this, it will purely be for fun. Most likely, the continuation of upkeep on this project will benefit only me unless I change around `FileReader` or `Program`. Before I change `FileReader` or `Program`, I will likely change `FileCruncher` to make things slightly easier on me. For instance, I would probably refactor the code so certain chunks are re-used as opposed to rewritten, writing out the potential problems found to a `.md` or `.txt` file, etc. 

## Access Database

The files I am parsing through are exported from an Access Database. I am trying to find errors amongst records in Access. I likely would have had an easier time if I had exported the Access Database in a way that encapsulated the text in double quotation marks, but I was able to work though it just fine regardless.

# Note on Test File

The `Sales-Invoicing.csv` has been edited heavily, and a lot of information and fields deleted for privacy issues.

# Word of Warning

I am using specific files for this. `FileCruncher` will break the program. I also wrote `FileCruncher` in a way that is slightly obscured to protect data structures, although, it may not be that hard to figure out; the important data is hidden. For `FileCruncher`, you will have to reorganize the class for it to work. This may not be the cleanest code, but it works for me and serves its current purpose. In the future, I may throw a GUI on this, and customize options for crunching the data. The way it is now, it will have to be changed to fit your specific use case. This part may come after I am satisfied with its level of data-crunching.