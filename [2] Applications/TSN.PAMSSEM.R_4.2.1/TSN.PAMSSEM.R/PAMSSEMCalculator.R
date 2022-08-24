##################################################
# PAMSSEM CALCULATOR
# Version   : 1.0
# Author    : MUSTAFA TOSUN, https://mustafatosun.net
# Date      : 2022-08-24
# Licence   : NONE
##################################################


windowWidth <- 150
Alternatives <- NA
IsOrdinal <- NA
Attributes <- NA
DecisionMatrix <- NA
Differences <- NA
Indices <- NA
Ds <- NA
OutrankingIndices <- NA
ConcordanceIndices <- NA
LocalDiscordanceIndices <- NA
OutrankingDegrees <- NA
EnteringFlows <- NA
LeavingFlows <- NA
NetFlows <- NA
FinalRanking1 <- NA
FinalRanking2 <- NA



writeObject <- function(x, title = "", wl = F) {
  t <- as.character(title)[1]
  if (nchar(t) > 0)
    writeLines(c(paste(c(rep(" ", windowWidth - nchar(t)), t), collapse = ""), paste(rep("-", windowWidth), collapse = "")))
  if (wl)
    writeLines(x)
  else
    print(x)
  writeLines(rep("", 3))
}

writeHeader <- function(title) {
  t <- as.character(title)[1]
  hr <- paste(rep("-", windowWidth), collapse = "")
  spaces <- rep(" ", as.integer((windowWidth - 2 - nchar(t)) / 2))
  writeLines(c(hr, paste(c("|", spaces, t, spaces, ifelse(nchar(t) %% 2 != 0, " ", ""), "|"), collapse = ""), hr, ""))
}

InitializePAMSSEM <- function(alternatives, isOrdinal, attributes_Name, attributes_Weight, attributes_q, attributes_p, attributes_v, attributes_y, attributes_isPositive, decisionMatrix) {
  writeHeader("AŞAMA 0 - PROBLEM OLUŞTURMA")
  
  Alternatives <<- as.vector(alternatives, mode = "character")
  IsOrdinal <<- ifelse(isOrdinal, T, F)
  Attributes <<- data.frame(
    Name = as.vector(attributes_Name, mode = "character"),
    Weight = as.vector(attributes_Weight, mode = "numeric"),
    q = as.vector(attributes_q, mode = "numeric"),
    p = as.vector(attributes_p, mode = "numeric"),
    v = as.vector(attributes_v, mode = "numeric"),
    y = ifelse(IsOrdinal, NA, as.vector(attributes_y, mode = "numeric")),
    IsPositive = as.vector(attributes_isPositive, mode = "logical"))
  #colnames(Attributes) <<- c("Ad", "Ağırlık", "Kayıtsızlık Eşiği", "Tercih Eşiği", "Reddetme Eşiği", "y Parametresi", "Pozitiflik")
  DecisionMatrix <<- matrix(decisionMatrix, nrow = length(Alternatives), ncol = length(Attributes$Name))
  rownames(DecisionMatrix) <<- Alternatives
  colnames(DecisionMatrix) <<- Attributes$Name
  
  writeObject(Alternatives, "ALTERNATİFLER")
  writeObject(ifelse(IsOrdinal, "Ordinal", "Kardinal"), "ORDİNALLLİK", T)
  writeObject(Attributes, "ÖZELLİKLER")
  writeObject(DecisionMatrix, "KARAR MATRİSİ")
}

SolvePAMSSEM_Stage1 <- function() {
  writeHeader("AŞAMA 1 - YEREL GEÇİŞ İNDİSİ")
  
  Differences <<- array(rep(NA, length(Alternatives) * length(Alternatives) * length(Attributes$Name)), dim = c(length(Alternatives), length(Alternatives), length(Attributes$Name)), dimnames = list(Alternatives, Alternatives, paste(Attributes$Name, " (", ifelse(Attributes$IsPositive, "+", "-"), ")", sep = "")))
  Indices <<- array(rep(NA, length(Alternatives) * length(Alternatives) * length(Attributes$Name)), dim = c(length(Alternatives), length(Alternatives), length(Attributes$Name)), dimnames = list(Alternatives, Alternatives, Attributes$Name))
  OutrankingIndices <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  if (IsOrdinal)
    for (i in 1:length(Alternatives))
      for (j in 1:length(Alternatives)) {
        if (i == j)
          next
        sum <- 0;
        for (k in 1:length(Attributes$Name)) {
          Differences[i, j, k] <<- (DecisionMatrix[i, k] - DecisionMatrix[j, k]) * ifelse(Attributes$IsPositive[k], 1, -1)
          if (Differences[i, j, k] <= -Attributes$p[k])
            sum <- sum + (Indices[i, j, k] <<- 0)
          else if (Differences[i, j, k] >= -Attributes$q[k])
            sum <- sum + (Indices[i, j, k] <<- 1)
          else if (-Attributes$p[k] < Differences[i, j, k] - Attributes$q[k] && Attributes$p[k] >= Attributes$q[k] && Attributes$q[k] >= 0)
            sum <- sum + (Indices[i, j, k] <<- (Differences[i, j, k] - Attributes$p[k]) / (Attributes$p[k] - Attributes$q[k]))
          else {
            sum <- sum + (Indices[i, j, k] <<- NaN)
            break
          }
        }
        OutrankingIndices[i, j] <<- sum
      }
  else
    for (i in 1:length(Alternatives))
      for (j in 1:length(Alternatives)) {
        if (i == j)
          next
        sum <- 0;
        for (k in 1:length(Attributes$Name)) {
          Differences[i, j, k] <<- (DecisionMatrix[i, k] - DecisionMatrix[j, k]) * ifelse(Attributes$IsPositive[k], 1, -1)
          if (Differences[i, j, k] >= 0)
            sum <- sum + (Indices[i, j, k] <<- 1)
          else if (Differences[i, j, k] >= -1)
            sum <- sum + (Indices[i, j, k] <<- .5)
        }
        OutrankingIndices[i, j] <<- sum
      }
  
  writeObject(Differences, "FARKLAR")
  writeObject(Indices, "İNDİSLER")
  writeObject(OutrankingIndices, "YEREL GEÇİŞ İNDİSLERİ")
}

SolvePAMSSEM_Stage2 <- function() {
  writeHeader("AŞAMA 2 - AHENK İNDİSİ")
  
  ConcordanceIndices <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  for (i in 1:length(Alternatives))
    for (j in 1:length(Alternatives)) {
      if (is.na(OutrankingIndices[i, j]))
        next
      sum = 0
      for (k in 1:length(Attributes$Name))
        sum <- sum + Attributes$Weight[k] * Indices[i, j, k]
      ConcordanceIndices[i, j] <<- sum
    }
  
  writeObject(ConcordanceIndices, "AHENK İNDİSİ")
}

SolvePAMSSEM_Stage3 <- function() {
  writeHeader("AŞAMA 3 - YEREL UYUMSUZLUK İNDİSİ")
  
  Ds <<- array(rep(NA, length(Alternatives) * length(Alternatives) * length(Attributes$Name)), dim = c(length(Alternatives), length(Alternatives), length(Attributes$Name)), dimnames = list(Alternatives, Alternatives, Attributes$Name))
  LocalDiscordanceIndices <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  if (IsOrdinal)
    for (i in 1:length(Alternatives))
      for (j in 1:length(Alternatives)) {
        if (is.na(OutrankingIndices[i, j]))
          next
        sum = 0
        for (k in 1:length(Attributes$Name)) {
          if (Differences[i, j, k] <= -Attributes$v[k])
            sum <- sum + (Ds[i, j, k] <<- 1)
          else if (Differences[i, j, k] >= -Attributes$p[k])
            sum <- sum + (Ds[i, j, k] <<- 0)
          else
            sum <- sum + (Ds[i, j, k] <<- -(Differences[i, j, k] + Attributes$p[k]) / (Attributes$v[k] - Attributes$p[k]))
        }
        LocalDiscordanceIndices[i, j] <<- sum
      }
  else
    for (i in 1:length(Alternatives))
      for (j in 1:length(Alternatives)) {
        if (is.na(OutrankingIndices[i, j]))
          next
        sum = 0
        for (k in 1:length(Attributes$Name)) {
          y <- (Attributes$y + 1) / 2
          if (Differences[i, j, k] < -y)
            sum <- sum + (Ds[i, j, k] <<- min(c(1, .2 * (1 + Attributes$Weight[k] / 2) * Differences[i, j, k] + y)))
        }
        LocalDiscordanceIndices[i, j] <<- sum
      }
  
  writeObject(Ds, "D")
  writeObject(LocalDiscordanceIndices, "YEREL UYUMSUZLUK İNDİSİ")
}

SolvePAMSSEM_Stage4 <- function() {
  writeHeader("AŞAMA 4 - GEÇİŞ DERECESİ")
  
  OutrankingDegrees <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  for (i in 1:length(Alternatives))
    for (j in 1:length(Alternatives)) {
      if (is.na(OutrankingIndices[i, j]))
        next
      product = ConcordanceIndices[i, j]
      for (k in 1:length(Attributes$Name))
        product <- product * (1 - Ds[i, j, k] ** 3)
      OutrankingDegrees[i, j] <<- ifelse(product >= 0 && product <= 1, product, NaN)
    }
  
  writeObject(OutrankingDegrees, "GEÇİŞ DERECESİ")
}

SolvePAMSSEM_Stage5 <- function() {
  writeHeader("AŞAMA 5 - GİRİŞ VE ÇIKIŞ AKIŞLARI")
  
  EnteringFlows <<- rep(NA, length(Alternatives))
  names(EnteringFlows) <<- Alternatives
  LeavingFlows <<- rep(NA, length(Alternatives))
  names(LeavingFlows) <<- Alternatives
  
  for (i in 1:length(Alternatives)) {
    sum1 <- 0
    sum2 <- 0
    for (j in 1:length(Alternatives)) {
      sum1 <- sum1 + ifelse(is.na(OutrankingDegrees[i, j]), 0, OutrankingDegrees[i, j])
      sum2 <- sum2 + ifelse(is.na(OutrankingDegrees[j, i]), 0, OutrankingDegrees[j, i])
    }
    EnteringFlows[i] <<- sum1
    LeavingFlows[i] <<- sum2
  }
  
  writeObject(EnteringFlows, "GİRİŞ AKIŞLARI")
  writeObject(LeavingFlows, "ÇIKIŞ AKIŞLARI")
}

SolvePAMSSEM_Stage6 <- function() {
  writeHeader("AŞAMA 6 - NET AKIŞLAR")
  
  NetFlows <<- EnteringFlows - LeavingFlows
  names(NetFlows) <<- Alternatives
  
  writeObject(NetFlows, "NET AKIŞLAR")
}

SolvePAMSSEM_Finalize1 <- function() {
  writeHeader("NİHAİ SIRALAMA - PAMSSEM I")
  
  FinalRanking1 <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  for (i in 1:length(Alternatives))
    for (j in i:length(Alternatives)) {
      order <- ""
      if (EnteringFlows[i] == EnteringFlows[j] && LeavingFlows[i] == LeavingFlows[j])
        order <- "I"
      else if ((EnteringFlows[i] > EnteringFlows[j] && LeavingFlows[i] <= LeavingFlows[j]) ||
               (EnteringFlows[i] == EnteringFlows[j] && LeavingFlows[i] < LeavingFlows[j]))
        order <- "P"
      FinalRanking1[i, j] <<- FinalRanking1[j, i] <<- order
    }
  
  writeObject(FinalRanking1)
}

SolvePAMSSEM_Finalize2 <- function() {
  writeHeader("NİHAİ SIRALAMA - PAMSSEM II")
  
  FinalRanking2 <<- list(I = list(), P = list())
  uq <- unique(NetFlows)
  
  if (length(uq) == 1)
    FinalRanking2$I <<- list(Alternatives)
  else if (length(uq) == length(NetFlows))
    FinalRanking2$P <<- list(names(sort(NetFlows, decreasing = T)))
  else {
    a <- 0
    b <- 0
    df <- data.frame(A = Alternatives, NF = NetFlows)
    for (i in 1:length(Alternatives))
      if (i < length(Alternatives)) {
        cI <- i
        cP <- i
        for (j in (i + 1):length(Alternatives)) {
          if (NetFlows[i] == NetFlows[j])
            cI <- c(cI, j)
          else
            cP <- c(cP, j)
        }
        if (length(cI) > 1)
          FinalRanking2$I[[a <- a + 1]] <<- Alternatives[cI]
        if (length(cP) > 1) {
          fr2p <- df[cP, ]$A[order(df$NF, decreasing = T)]
          FinalRanking2$P[[b <- b + 1]] <<- fr2p[which(!is.na(fr2p))]
        }
      }
  }
  
  if (length(FinalRanking2$I) > 0)
    writeObject(sapply(FinalRanking2$I, function (x) paste(x, collapse = " I ")), wl = T)
  if (length(FinalRanking2$P) > 0)
    writeObject(sapply(FinalRanking2$P, function (x) paste(x, collapse = ifelse(length(uq) == length(NetFlows), " > ", " P "))), wl = T)
}