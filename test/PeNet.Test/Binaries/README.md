Information to:
1. pidgin_manipulated_GroupIconDirectory.exe:
   - Contains 5 GroupIconDirectories with [9,0,2,9,0] GroupIconDirectoryEntries.
   - These consist of:
       - The normal pidgin.exe GroupIconDirectory.
       - A GroupIconDirectory of random bytes.
       - A shortened version of the first GroupIconDirectory with only 2 GroupIconDirectoryEntries.
       - A version of the first GroupIconDirectory extended with random bytes.
       - A shortened version of the first GroupIconDirectory with only 4 bytes.

2. pidgin_manipulated_GroupIconDirectory_withNullIcon.exe:
   - Contains 5 GroupIconDirectories with [9,0,2,9,0] GroupIconDirectoryEntries and an additionally dummy Icon with ID 17473.
   - These consist of:
       - The normal pidgin.exe GroupIconDirectory.
       - A GroupIconDirectory of random bytes.
       - A shortened version of the first GroupIconDirectory with only 2 GroupIconDirectoryEntries.
       - A version of the first GroupIconDirectory extended with random bytes.
       - A shortened version of the first GroupIconDirectory with less than a whole GroupIconDirectoryEntry.

3. pidgin_manipulated_Icons.exe:
   - Contains 15 Icons:
     - Icon 1-9 are the normal Icons from pidgin.exe. [Displayable, Extractable]
     - Icon 10 is a shortened version of Icon 9. [Extractable]
     - Icon 11 is a version of Icon 9 with random byte in his header. [Extractable]
     - Icon 12 is a shortened version of Icon 9 which is shorter than the header.
     - Icon 13 is a version of Icon 9 extended with random bytes. [Displayable, Extractable]
     - Icon 14 consists of only 1 byte.
     - Icon 15 contains an image consisting of random bytes with the header of Icon 9. [Displayable, Extractable]